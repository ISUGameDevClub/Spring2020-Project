using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{

    // 1) public/private 2) type 3) name 4) optional value

    public int lives = 3;
    [SerializeField] private float speed = 5f;
    public bool isPlayerAlive = true;

    public float horizontalInput;
    public float verticalInput;

    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame, 60 frames per second
    void Update()
    {
        Movement();
        Shooting();
    }
    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        if (transform.position.x > 4.5f)
        {
            transform.position = new Vector3(4.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -4.5f)
        {
            transform.position = new Vector3(-4.5f, transform.position.y, 0);
        }
        if (transform.position.y > 100f)
        {
            transform.position = new Vector3(transform.position.x, 0, 100);
        }
        else if (transform.position.z < 0f)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Instantiate(what to instantiate, where to instantiate, which rotation to instantiate in) 
            Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}