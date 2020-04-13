using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hook;
    public GameObject hookHolder;
    public GameObject hookedObj;

    public float hookSpeed;
    public float playerSpeed;
    public float maxDistance;

    private float currentDistance;
    public static bool fired;
    public bool hooked;

    // Update is called once per frame
    void Update()
    {
        //to fire
        if (Input.GetKey(KeyCode.E) && fired == false)
        {
            fired = true;
        }

        //rendering the rope
        if (fired)
        {
            LineRenderer rope = hook.GetComponent<LineRenderer>();
            rope.SetVertexCount(2);
            rope.SetPosition(0, hookHolder.transform.position);
            rope.SetPosition(1, hook.transform.position);
        }
        
        //returning the hook
        if (fired == true && hooked == false)
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
            {
                ReturnHook();
            }
        }

        //pulling the player to hooked object
        if (hooked == true && fired == true)
        {
            hook.transform.parent = hookedObj.transform;
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            this.GetComponent<Rigidbody>().useGravity = false;

            //returning hook once close enough and climbing the object
            if (distanceToHook < 1)
            {
                ReturnHook();
                /*
                if(GetComponent<PlayerController>().isGrounded())
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 14f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 18f);
                }

                StartCoroutine("Climb");
                */
            }
        }
        else
        {
            hook.transform.parent = hookHolder.transform;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    //climbing method
    IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.1f);
        ReturnHook();
    }

    //return hook method
    void ReturnHook()
    {
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;

        LineRenderer rope = hook.GetComponent<LineRenderer>();
        rope.SetVertexCount(0);

    }

}
