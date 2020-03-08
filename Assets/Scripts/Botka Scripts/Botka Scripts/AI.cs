using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public enum EntityStatus
    {
        None, NotSpawned, Dead, Patrol, Idle, PursuingTarget

    }

   [SerializeField] private GameObject target;
    [SerializeField] private NavMeshAgent navMeshAgent;


    private EntityStatus entityStatus;


    private bool canMove;
    private bool canAttack;
    private bool interrupt;

  
    

    private Coroutine attackCoroutine;

    [Header("Movement Options")] 
    [SerializeField] [Range(0f,10f)] private float velocityModiferier;
    [SerializeField] [Range(0f, 1000f)] private float detectionRange;

    [Header("Debug- DO NOT SET")]
   public EntityStatus entityStatusDubug;
    public Vector3 entityLocation;
    public bool canMoveDebug;
    public bool canAttackDebug;
    public bool interruptedActive;

    void Awake()
    {
        canAttack = false;
        canMove = false;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true; // ensures that movement code is not execute until the first frame
        canMove = true;
        navMeshAgent.destination = target.transform.position;
        

        
    }

    // Update is called once per frame
    void fixedUpdate()
    {
        dataValidation(); // first call
        if (interrupt)
        {
            if (attackCoroutine != null)
            {

                StopCoroutine(attackCoroutine);
                attackCoroutine = null;

            }

            // stop movement

            
        }

        if (canMove)
        {
            if (target != null)
            {
                if (checkIfInRangeOfTarget())
                {
                    moveTowardsTarget();
                }
            }
        }

        updateVariables(); // last call
        
    }

    private bool checkIfInRangeOfTarget()
    {
        return true;

    }

    private void moveTowardsTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);

    }


    private IEnumerator executeAttack()
    {
        
        yield return new WaitForSeconds(0f);
        attackCoroutine = null;

    }

    private void dataValidation()
    {
        if (target == null)
        {
            // handle
        }

        if (canAttack == false)
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

        

    }

    private void updateVariables()
    {
        entityStatusDubug = entityStatus;
        entityLocation = gameObject.transform.position;
        canMoveDebug = canMove;
        canAttackDebug = canAttack;
        interruptedActive = interrupt;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // this makes sure that if the target and the target objects collider are on different hiearchy levels that it still will return true 
        //by getting the upmost parent and then comparing
        if (other.transform.root.gameObject == target.transform.root.gameObject) 
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(executeAttack());
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    //setters and getters here

}
