using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float slideForce;
    public float gravityScale;
    public float wallRunningSpeedBoost;
    public float wallRunningGravity;
    public float MinSlideTime;
    public float slideControl;
    public float runControl;
    public float airControl;

    // Power Ups
    public float lowGravity;
    public float SpeedBoost; 
    public float doubleJump;
    public float slowTime;

    // Power Up variables
    public float lowGravityModifier;
    public float speedBoostModifier;

    public Vector3 desiredScale;

    private CameraController cc;
    private GameObject cr;
    private Rigidbody rb;
    private Animator camAnim;

    private float floorRaycastDistance; // how far the player can be off the ground and still jump
    private float wallRaycastDistance;

    private bool crouching;
    private bool running;
    public int wallRunning;
    private int wallJumpDirection;
    private Vector3 wallJumpAngle;
    private float wallJumpTimer;
    public float wantedAngle;
    private float playerControl;

    private bool inWind;
    private Vector3 windDirection;
    private float windPower;

    private float grav = -9.81f;

    private bool disableRight;
    private bool disableLeft;
    private bool canStand;

    private bool hasSecondJump;

    void Start()
    {
        cc = FindObjectOfType<CameraController>();
        cr = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody>();
        camAnim = transform.GetComponentInChildren<Animator>();
        floorRaycastDistance = 1.1f;
        wallRaycastDistance = .9f;
        playerControl = .05f;
        rb.useGravity = false;
    }

    void Update()
    {
        if (lowGravity > 0)
            lowGravity -= Time.deltaTime;

        if (SpeedBoost > 0)
            SpeedBoost -= Time.deltaTime;

        if (doubleJump > 0)
            doubleJump -= Time.deltaTime;

        if (slowTime > 0)
            slowTime -= Time.deltaTime;




        if (!FindObjectOfType<PauseMenu>().gamePaused)
        {
            if (slowTime > 0)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else
        {
            wallJumpTimer = 0;
        }

        if ((isGrounded() || wallJumpDirection != 0) && transform.localScale.y == desiredScale.y)
            Jump();

        if (doubleJump > 0 && hasSecondJump && !isGrounded() && wallJumpDirection == 0 && transform.localScale.y == desiredScale.y)
            DoubleJump();

        if (Input.GetKey(KeyCode.LeftControl))
            running = false;
        else
            running = true;

        if (transform.localScale.y > desiredScale.y * .95f)
            transform.localScale = desiredScale;
        else if (transform.localScale.y < desiredScale.y * .55f)
            transform.localScale = new Vector3(desiredScale.x, desiredScale.y * .5f, desiredScale.z);
    }

    private void FixedUpdate()
    {
        Vector3 gravity = Vector3.zero;

        if (!crouching && Input.GetButton("Slide") && isGrounded() && (transform.localScale.y == 1))
        {
            crouching = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (rb.velocity.magnitude > 6)
                rb.AddForce(slideForce * transform.forward, ForceMode.Impulse);
            canStand = false;
            StartCoroutine(AllowStanding());
        }
        else if (crouching && !Input.GetButton("Slide") && isGrounded() && NoObjectAbove() && canStand)
            crouching = false;
        else if (!isGrounded())
            crouching = false;

        if (!crouching)
        {
            gravity = grav * gravityScale * Vector3.up;
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, .2f);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(desiredScale.x, desiredScale.y * .5f, desiredScale.z), .2f);
            gravity = grav * gravityScale * 3 * Vector3.up;
        }

        if (lowGravity > 0)
            gravity *= lowGravityModifier;

        rb.AddForce(gravity, ForceMode.Acceleration);
        WallRun();
        Move();

        transform.localRotation = Quaternion.AngleAxis(wantedAngle, transform.up);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }

    private void Move()
    {
        float HorMovement = Input.GetAxis("Horizontal");
        float VerMovement = Input.GetAxis("Vertical");

        if (disableRight && HorMovement > 0)
            HorMovement = 0;

        if (disableLeft && HorMovement < 0)
            HorMovement = 0;

        if (wallRunning != 0)
            HorMovement = 0;

        Vector3 inputDirection = rb.transform.TransformDirection(new Vector3(HorMovement, 0, VerMovement));
        if (new Vector3(HorMovement, 0, VerMovement).magnitude > 1 || wallRunning != 0)
            inputDirection = rb.transform.TransformDirection(new Vector3(HorMovement, 0, VerMovement)).normalized;

        Vector3 finalVelocity;

        if (crouching)
            finalVelocity = inputDirection * crouchSpeed;
        else if(running)
            finalVelocity = inputDirection * runSpeed;
        else
            finalVelocity = inputDirection * walkSpeed;

        if (wallRunning != 0)
            finalVelocity *= (1 + wallRunningSpeedBoost);

        if (SpeedBoost > 0)
            finalVelocity *= (1 + speedBoostModifier);

        finalVelocity = new Vector3(finalVelocity.x, rb.velocity.y, finalVelocity.z);

        if(inWind)
        {
            finalVelocity += windDirection.normalized * windPower;
        }

        finalVelocity = new Vector3(finalVelocity.x * .9f, finalVelocity.y, finalVelocity.z * .9f);

        if(isGrounded() && crouching)
            playerControl = slideControl;
        else if(isGrounded())
            playerControl = runControl;
        else
            playerControl = airControl;

        Vector3 finalHorVelocity = Vector3.Lerp(rb.velocity, new Vector3(finalVelocity.x, 0, finalVelocity.z), playerControl * Time.deltaTime * 10);

        rb.velocity = new Vector3(finalHorVelocity.x , finalVelocity.y, finalHorVelocity.z);
    }

    private void Jump()
    {
        if (doubleJump > 0)
            hasSecondJump = true;

        if (Input.GetButtonDown("Jump"))
        {
            wallJumpTimer = 0;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (wallJumpDirection == 0)
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            else if (wallJumpDirection == 1 && WallOnRight())
            {
                rb.AddForce(0, jumpForce * .8f, 0, ForceMode.Impulse);
                rb.AddForce(jumpForce * 1.5f * -wallJumpAngle, ForceMode.Impulse);
                disableRight = true;
                StartCoroutine(EnableRight());
            }
            else if (wallJumpDirection == 2 && WallOnLeft())
            {
                rb.AddForce(0, jumpForce * .8f, 0, ForceMode.Impulse);
                rb.AddForce(jumpForce * 1.5f * wallJumpAngle, ForceMode.Impulse);
                disableLeft = true;
                StartCoroutine(EnableLeft());
            }
        }
    }

    private void DoubleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            hasSecondJump = false;
        }
    }

    private void WallRun()
    {
        if (wallRunning != 1 && !isGrounded() && Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") > 0 && !disableRight && WallOnRight() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallRunningGravity, rb.velocity.z);
            wallRunning = 1;
            wallJumpDirection = 1;
            wallJumpTimer = .25f;
            wallJumpAngle = new Vector3(transform.right.x, transform.right.y, transform.right.z);
            cc.SetCameraRotaterWallRunningRight();
        }
        else if(wallRunning == 1 && !isGrounded() && Input.GetAxis("Horizontal") >= 0 && Input.GetAxis("Vertical") > 0 && !disableRight && WallOnRight() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallRunningGravity, rb.velocity.z);
            wallRunning = 1;
            wallJumpDirection = 1;
            wallJumpTimer = .25f;
            wallJumpAngle = new Vector3(transform.right.x, transform.right.y, transform.right.z);
            cc.SetCameraRotaterWallRunningRight();
        }
        else if (wallRunning != 2 && !isGrounded() && Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") > 0 && !disableLeft && WallOnLeft() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallRunningGravity, rb.velocity.z);
            wallRunning = 2;
            wallJumpDirection = 2;
            wallJumpTimer = .25f;
            wallJumpAngle = new Vector3(transform.right.x, transform.right.y, transform.right.z);
            cc.SetCameraRotaterWallRunningLeft();
        }
        else if (wallRunning == 2 && !isGrounded() && Input.GetAxis("Horizontal") <= 0 && Input.GetAxis("Vertical") > 0 && !disableLeft && WallOnLeft() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallRunningGravity, rb.velocity.z);
            wallRunning = 2;
            wallJumpDirection = 2;
            wallJumpTimer = .25f;
            wallJumpAngle = new Vector3(transform.right.x, transform.right.y, transform.right.z);
            cc.SetCameraRotaterWallRunningLeft();
        }
        else
        {
            cr.transform.rotation = new Quaternion(0, 0, 0, 0);
            wantedAngle = cc.currentLookingPos.x;
            wantedAngle %= 360;
            if (wantedAngle < 0)
                wantedAngle = 360.0f - Mathf.Abs(wantedAngle);
            wallRunning = 0;
            if(wallJumpTimer == 0)
            {
                wallJumpDirection = 0;
            }
        }

        camAnim.SetInteger("Wall Run State", wallRunning);
    }

    private IEnumerator EnableRight()
    {
        yield return new WaitForSeconds(.3f);
        disableRight = false;
    }

    private IEnumerator EnableLeft()
    {
        yield return new WaitForSeconds(.3f);
        disableLeft = false;
    }
    private IEnumerator AllowStanding()
    {
        yield return new WaitForSeconds(MinSlideTime);
        canStand = true;
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, floorRaycastDistance);
    }

    private bool NoObjectAbove()
    {
        bool check1 = !Physics.Raycast(transform.position, Vector3.up, 1.25f);
        bool check2 = !Physics.Raycast(new Vector3(transform.position.x + .6f, transform.position.y, transform.position.z), Vector3.up, 1.25f);
        bool check3 = !Physics.Raycast(new Vector3(transform.position.x - .6f, transform.position.y, transform.position.z), Vector3.up, 1.25f);
        bool check4 = !Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + .6f), Vector3.up, 1.25f);
        bool check5 = !Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - .6f), Vector3.up, 1.25f);


        return check1 && check2 && check3 && check4 && check5;
    }

    private bool WallOnRight()
    {
        RaycastHit hit;
        bool check1 = Physics.Raycast(new Vector3 (transform.position.x, transform.position.y + .5f, transform.position.z), transform.right, out hit, wallRaycastDistance);
        bool check2 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), transform.right, wallRaycastDistance);
        if (check1)
        {
            wantedAngle = Vector3.Angle(hit.normal, -Vector3.right);
            Vector3 cross = Vector3.Cross(hit.normal, -Vector3.right);
            if (cross.y > 0)
                wantedAngle *= -1;
            wantedAngle %= 360;
            if (wantedAngle < 0)
                wantedAngle = 360.0f - Mathf.Abs(wantedAngle);
        }
        return check1 && check2;
    }

    private bool WallOnLeft()
    {
        RaycastHit hit;
        bool check1 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), -transform.right, out hit, wallRaycastDistance);
        bool check2 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), -transform.right, wallRaycastDistance);
        if (check1)
        {
            wantedAngle = Vector3.Angle(hit.normal, Vector3.right);
            Vector3 cross = Vector3.Cross(hit.normal, Vector3.right);
            if (cross.y > 0)
                wantedAngle *= -1;
            wantedAngle %= 360;
            if (wantedAngle < 0)
                wantedAngle = 360.0f - Mathf.Abs(wantedAngle);
        }
        return check1 && check2;
    }

    public void EnterWind(Vector3 dir, float power)
    {
        inWind = true;
        windDirection = dir;
        windPower = power;
    }

    public void ExitWind()
    {
        inWind = false;
        windDirection = Vector3.zero;
        windPower = 0;
    }
}
