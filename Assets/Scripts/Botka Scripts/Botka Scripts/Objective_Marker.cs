using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
       @Author - Jake Botka (Lead), Josh N (Assited, Learning), DO NOT alter without permission of present, team leader, or author
   */
public class Objective_Marker : MonoBehaviour
{
    /*
        @Author - Jake Botka (Lead), Josh N (Assited, Learning), DO NOT alter without permission of present, team leader, or author
    */


    // private int priority; // check out ontriggerenter and we will deal with priority there

    //private Player player; // delete this

    private Object triggerEvent; // this is a place holder for now
    private Transform markerTransform;

    private Coroutine triggerEventCoroutine;

    private Vector3 markerPosition;
    private bool markDestroy; // mark script for destruction
    private bool eventReady;

    private long instanceId;
    [Header("Marker Options")]
    [SerializeField] private bool debugToConsole;

    [Header("Debug")]
    [SerializeField] private  bool eventTriggered; 
    [SerializeField] private  bool markerActivatedByTarget;
  [SerializeField]  private readonly bool scriptActivated = true; // readonly alot like final but can be set whenever but once set is permanent


    void Awake()
    {
        
        instanceId = -1;
        markDestroy = false;
        eventReady = false;

    }

    void Start()
    {
        instanceId = (long)gameObject.GetInstanceID();
        markerTransform = gameObject.transform;
        markerPosition = markerTransform.position;

        dataValidation();

    }

    void Update()
    {
        dataValidation(); // first call

        updateVariables(); //;ast call

    }

    private void dataValidation()
    {
        if (markDestroy)
        {
            Destroy(gameObject.GetComponentInChildren<Objective_Marker>()); // destroys script
        }

        if (gameObject.GetComponentInChildren<Collider>() == null) // makes sure that the gamobject has a trigger collider 
        {

            Debug.LogError("Objective marker missing trigger Collider");
        }
        else
        {
            if (gameObject.GetComponentInChildren<Collider>().isTrigger == false) // makes sure the collider has istrigger set
            {
                gameObject.GetComponentInChildren<Collider>().isTrigger = true;
                Debug.LogWarning("Trigger collider was mannualy set in script due to the trigger being set to false fix this in spector");
            }
        }
    }

    private void updateVariables()
    {
        
    }

    public void activateTriggerEvent()
    {
        // start mission or anything from trigger even with player

        if (triggerEventCoroutine == null)
        {
            triggerEventCoroutine = StartCoroutine(handleEvent());
        }
    }

    public IEnumerator handleEvent()
    {
        yield return new WaitForEndOfFrame(); // forces this event to wait till the ned of update frame execution to prevent visual bugs
        yield return new WaitUntil(() => eventReady == true); // waits until vairable is true 

        // put handle code here

        triggerEventCoroutine = null; // resets the variable telling that the coroutine no longer exists. this is important for singe activation at a time
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" || collider.GetComponentInChildren<PlayerController>())
        {
            activateTriggerEvent();
           
            // start mission or event
        }
        else if (collider.GetComponentInChildren<Objective_Marker>()) // 2 markers taking up the same space
        {
            Destroy(collider.gameObject); // destroys duplicate
            Debug.LogWarning("Duplicates objective marker collider with one another. Chek inspector to see that no objective marker occupy the same trigger collision space");
        }

        if (debugToConsole)
        {
            Debug.Log(collider.gameObject.name + " has entered the objective markers trigger radius");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (debugToConsole)
        {
            Debug.Log(collider.gameObject.name + " has left the objective markers trigger radius");
        }

    }


 
    private void OnDestroy()
    {
        if (debugToConsole)
        {
            //Debug.Log(gameObject.transform.root.name + " marker script was destroyed."); 
            //if reporting script death dont get parent name get name that the script is attached too so they know where to look
            Debug.Log(gameObject.name + " marker script was destroyed.");
        }
    }
}
