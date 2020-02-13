using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float slideForce;
    public float gravityScale;
    public float wallRunningSpeedBoost;
    public float MinSlideTime;
    public float slideControl;
    public float runControl;
    public float airControl;


    private Rigidbody rb;
    private float floorRaycastDistance; // how far the player can be off the ground and still jump
    private float wallRaycastDistance;

    private bool crouching;
    private bool running;
    private int wallRunning;
    private float playerControl;

    private bool inWind;
    private Vector3 windDirection;
    private float windPower;
    private Animator camAnim;
    private float grav = -9.81f;

    private bool disableRight;
    private bool disableLeft;
    private bool canStand;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camAnim = transform.GetComponentInChildren<Animator>();
        floorRaycastDistance = 1.1f;
        wallRaycastDistance = .9f;
        playerControl = .05f;
        rb.useGravity = false;
    }

    void Update()
    {
        if ((isGrounded() || wallRunning != 0) && transform.localScale.y == 1)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            running = !running;

        if (!crouching && Input.GetKey(KeyCode.LeftControl) && isGrounded() && (transform.localScale.y == 1))
        {
            crouching = true;
            rb.AddForce(slideForce * transform.forward, ForceMode.Impulse);
            canStand = false;
            StartCoroutine(AllowStanding());
        }
        else if(crouching && !Input.GetKey(KeyCode.LeftControl) && isGrounded() && NoObjectAbove() && canStand)
            crouching = false;
        else if (!isGrounded())
            crouching = false;

        if (!crouching)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), .2f);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, .5f, 1), .2f);

        if (transform.localScale.y > .95f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (transform.localScale.y < .55f)
            transform.localScale = new Vector3(1, .5f, 1);
    }

    private void FixedUpdate()
    {
        Vector3 gravity = Vector3.zero;
        if (!crouching)
            gravity = grav * gravityScale * Vector3.up;
        else
            gravity = grav * gravityScale * 3 * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        WallRun();
        Move();

        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }

    private void Move()
    {
        float HorMovement = Input.GetAxisRaw("Horizontal");
        float VerMovement = Input.GetAxisRaw("Vertical");

        if (disableRight && HorMovement > 0)
            HorMovement = 0;

        if (disableLeft && HorMovement < 0)
            HorMovement = 0;

        Vector3 finalVelocity;

        if (crouching)
            finalVelocity = rb.transform.TransformDirection(new Vector3(HorMovement, 0, VerMovement).normalized * crouchSpeed);
        else if(running)
            finalVelocity = rb.transform.TransformDirection(new Vector3(HorMovement, 0, VerMovement).normalized * runSpeed);
        else
            finalVelocity = rb.transform.TransformDirection(new Vector3(HorMovement, 0, VerMovement).normalized * walkSpeed);

        if (wallRunning != 0)
            finalVelocity *= (1 + wallRunningSpeedBoost);

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
        Debug.Log(playerControl * Time.deltaTime * 100);

        rb.velocity = new Vector3(finalHorVelocity.x , finalVelocity.y, finalHorVelocity.z);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (wallRunning == 0)
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            else if (wallRunning == 1)
            {
                rb.AddForce(0, jumpForce * .8f, 0, ForceMode.Impulse);
                rb.AddForce(jumpForce * 2 * -transform.right, ForceMode.Impulse);
                disableRight = true;
                StartCoroutine(EnableRight());
            }
            else if (wallRunning == 2)
            {
                rb.AddForce(0, jumpForce * .8f, 0, ForceMode.Impulse);
                rb.AddForce(jumpForce * 2 * transform.right, ForceMode.Impulse);
                disableLeft = true;
                StartCoroutine(EnableLeft());
            }
        }
    }

    private void WallRun()
    {
        if (!isGrounded() && Input.GetKey(KeyCode.D) && !disableRight && WallOnRight() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, .2f, rb.velocity.z);
            wallRunning = 1;
        }
        else if (!isGrounded() && Input.GetKey(KeyCode.A) && !disableLeft && WallOnLeft() && rb.velocity.y <= 5f)
        {
            rb.velocity = new Vector3(rb.velocity.x, .2f, rb.velocity.z);
            wallRunning = 2;
        }
        else
            wallRunning = 0;

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
        return !Physics.Raycast(transform.position, Vector3.up, 1.25f);
    }

    private bool WallOnRight()
    {
        bool check1 = Physics.Raycast(new Vector3 (transform.position.x, transform.position.y + .5f, transform.position.z), transform.right, wallRaycastDistance);
        bool check2 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), transform.right, wallRaycastDistance);
        return check1 && check2;
    }

    private bool WallOnLeft()
    {
        bool check1 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), -transform.right, wallRaycastDistance);
        bool check2 = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), -transform.right, wallRaycastDistance);
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
