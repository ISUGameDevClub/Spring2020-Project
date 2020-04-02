using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float height;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;

    private bool goingToTarget;
    private bool frozen;

    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, startPosition.y + height,startPosition.z);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!frozen)
        {
            if (goingToTarget)
            {
                if (Vector3.Distance(transform.position, endPosition) < .1f)
                    transform.position = endPosition;
                else
                {
                    Vector3 offset = (endPosition - transform.position).normalized * speed * Time.deltaTime;
                    rb.MovePosition(transform.position + offset);
                }
            }
            else if (!goingToTarget)
            {
                if (Vector3.Distance(transform.position, startPosition) < .1f)
                    transform.position = startPosition;
                else
                {
                    Vector3 offset = (startPosition - transform.position).normalized * speed * Time.deltaTime;
                    rb.MovePosition(transform.position + offset);
                }
            }
        }
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position.y > transform.position.y)
            {
                if (!goingToTarget)
                    goingToTarget = true;
                other.transform.parent = gameObject.transform;
                other.transform.localScale = new Vector3(1 / 3f, 1, 1 / 1 / 3f);
                other.GetComponent<PlayerController>().desiredScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);
            }
            else
                frozen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position.y > transform.position.y)
            {
                if (goingToTarget)
                    goingToTarget = false;
                other.transform.parent = null;
                other.transform.localScale = new Vector3(1, 1, 1 / 1);
                other.GetComponent<PlayerController>().desiredScale = new Vector3(1, 1, 1);
            }
            else
                frozen = false;
        }
    }
}
