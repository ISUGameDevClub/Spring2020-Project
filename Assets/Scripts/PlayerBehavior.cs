using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    // 1) public/private 2) type 3) name 4) optional value

    public int lives = 3;
    public int score = 0;
    [SerializeField] private float speed = 5f;
    public bool isPlayerAlive = true;

    public float horizontalInput;
    public float verticalInput;

    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Lives: " + lives + "Score:" + score);
    }

    // Update is called once per frame, 60 frames per second
    void Update()
    {


    }
    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
        if (transform.position.y > 1f)
        {
            transform.position = new Vector3(transform.position.x, 1f, 0);
        }
        else if (transform.position.y < -5f)
        {
            transform.position = new Vector3(transform.position.x, -5f, 0);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            // Instantiate(what to instantiate, where to instantiate, which rotation to instantiate in)
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    public void GetDamaged()
    {
        lives--;
        if (lives < 1)
        {
            Destroy(this.gameObject);
        }
        Debug.Log("Lives " + lives + "Score: " + score);
    }
}