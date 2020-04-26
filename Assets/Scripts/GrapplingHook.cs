using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hook;
    public GameObject hookHolder;
    private GameObject hookedObj;
    private LineRenderer rope;
    public float hookSpeed;
    public float playerSpeed;
    public float maxDistance;
    private float currentDistance;
    public bool fired;
    public bool hooked;
    private KeyCode fireHook;
    private Rigidbody rb;
    private Coroutine x;
    private Coroutine y;
    public bool canFire;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canFire = true;
        fireHook = KeyCode.E;

        rope = hook.GetComponent<LineRenderer>();
        rope.SetVertexCount(2);
        rope.SetPosition(0, hookHolder.transform.position);
        rope.SetPosition(1, hook.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //to fire
        if (Input.GetKey(fireHook) && canFire)
        {
            canFire = false;
            fired = true;
        }

        //to allow and disallow gun fire
        if (fired == true)
        {
            GetComponent<Gun>().disAllowFire();
        }
        if(fired == false)
        {
            GetComponent<Gun>().allowFire();
        }

        if(GetComponent<PlayerController>().isGrounded() == true && fired == false)
        {
            hooked = false;
        }

        //rendering the rope POTENTIAL ISSUE
        DrawRope();

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
            hook.transform.parent = null;
            Vector3 pos = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerSpeed);
            rb.MovePosition(pos);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            gameObject.GetComponent<Rigidbody>().useGravity = false;

            //returning hook once close enough and climbing the object
            if (distanceToHook < 1.85f)
            {
                if (x == null)
                {
                    x = StartCoroutine(delay());
                    ReturnHook();
                }
            }
        }
        else
        {
            hook.transform.parent = hookHolder.transform;
            //gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void HookReleaseWrapper()
    {
        StartCoroutine(HookRelease());
    }

    public IEnumerator HookRelease()
    {
        yield return new WaitForSeconds(2);
        ReturnHook();
    }

    public void ReturnHook()
    {
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
        if (y == null)
        {
            y= StartCoroutine(fireRate());
        }
    }

    private void DrawRope()
    {
        if (fired)
        {
            rope = hook.GetComponent<LineRenderer>();
            rope.SetVertexCount(2);
            rope.SetPosition(0, hookHolder.transform.position);
            rope.SetPosition(1, hook.transform.position);
        }
        else
        {
            rope.positionCount=0;
        }
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(.70f);
        yield return new WaitForEndOfFrame();
        x = null;
    }

    public IEnumerator fireRate()
    {
        yield return new WaitForSeconds(1.75f);
        yield return new WaitForEndOfFrame();
        canFire = true;
        y = null;
    }

    public GameObject getHookedObj()
    {
        return hookedObj;
    }

    public void setHookedObj(GameObject newHookedObject)
    {
        hookedObj = newHookedObject;
    }
}
