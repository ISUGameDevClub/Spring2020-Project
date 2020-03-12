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
    [Header("Enemy Attributes object data container")]
    [Tooltip("This object holds all of the data attributes for the script to read from. " )]
    [SerializeField] private EnemyTypeAttributes enemyAttributes;

    [SerializeField]private Animator animator;

    private EntityStatus entityStatus;
    private bool canMove; //variable determines if AI is locked in position
    private bool canAttack; 
    private bool interrupt; // variable determinnes if AI should halt current coroutine

    private bool activateAnimationUssage; // debug perpuses
  
  
    

    private Coroutine attackCoroutine; // multithreaded coroutine that execute parrellel to the update method when called. Must be called ussualy only once per execution

    [Header("Movement Options")] 
    [Range(0f,10f)] private float velocityModiferier;
  [Range(0f, 1000f)] private float detectionRange;

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
        interrupt = false;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true; // ensures that movement code is not execute until the first frame
        canMove = true;
        navMeshAgent.destination = target.transform.position;
        dataValidation();
        

        
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
        if (target != null)
        {
            if (navMeshAgent.destination != target.transform.position)
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " : Target Was null trying to fix in script");
            ///
            Debug.LogError(gameObject.name + " : Target is null, could NOT fix in script. Fix in inspector");
        }

    }

    private void stopMovement()
    {
        navMeshAgent.SetDestination(target.transform.position);

    }


    private IEnumerator executeAttack() // this should call the attack animation
    {
        
        yield return new WaitForSeconds(0f); // replace this line
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

        if (animator == null) // animator variable is null, lets chekc to see if it exists
        {
            activateAnimationUssage = false;
            Debug.LogWarning("Animator is missing attemting to fix in script");

            if (gameObject.GetComponent<Animator>() != null) // checks its existance
            {
                animator = gameObject.GetComponent<Animator>();
                Debug.LogWarning("Found animator, animator is now fixed. Check inspector to see if this could be prevented next time").
            }
            else
            {
                Debug.LogError("Animator is missing, could NOT fix in script");
            }
        }
        else // animator is not null and exists
        {
            activateAnimationUssage = true;
        }

        

    }

    private void updateVariables() // mostly for debug variables
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
        Debug.Log(gameObject.name + " was destroyed");
        
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
