using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Enemy), typeof(Collider))]
public class AI : MonoBehaviour
{
    public enum EntityStatus
    {
        None, NotSpawned, Dead, Patrol, Idle, PursuingTarget

    }

    public const string TAG = "AI";

    private const string ANIMATOR_STATE_IDLE = "idle";
    private const string ANIMATOR_STATE_WALKING = "walking";
    private const string ANIMATOR_STATE_RUNNING = "running";
    private const string ANIMATOR_STATE_MEELE_ONE = "MeeleOne";
    private const string ANIMATOR_STATE_MEELE_TWO = "MeeleTwo";
    private const string ANIMATOR_STATE_MEELE_THREE = "MeeleThree";
    private const string ANIMATOR_STATE_GUN_ONE = "GunOne";
    private const string ANIMATOR_STATE_GUN_TWO = "GunTwo";
    private const string ANIMATOR_STATE_GUN_THREE = "GunThree";


    private GameObject weapon;
    public float stoppingDistance =10;

    private Gun gun;
    public enum AIAttackType
    {
        Gun, Physical, None
    }

    [Header("Essentials")]

    [SerializeField] private GameObject target;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private AIAttackType attackType;

    [Header("Enemy Attributes object data container")]

    [Tooltip("This object holds all of the data attributes for the script to read from. " )]
    [SerializeField] private EnemyTypeAttributes enemyAttributes;

    [Header("Animation settings")]

    [SerializeField]private Animator animator;

    private EntityStatus entityStatus;
    private bool canMove; //variable determines if AI is locked in position
    private bool canAttack; 
    private bool interrupt; // variable determinnes if AI should halt current coroutine

    private bool activateAnimationUssage; // debug perpuses

    private AIAttackType currentAttackExecution;
  
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
    public float navMeshPathfindingSpeed;
    /**
     * 
     */
    void Awake()
    {
        canAttack = false;
        canMove = false;
        interrupt = false;
        
        
    }
    /**
     * 
     */
    void Start()
    {
        if (stoppingDistance <= 0)
        {
            stoppingDistance = 5;
        }
        canMove = true;

        navMeshAgent.SetDestination(target.transform.position);
        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;
        
        dataValidation();
        gun = null;
        if ((gun = gameObject.GetComponentInChildren<Gun>()) != null) // chekcs if null then assigns it inside conditional statement
        { }
        

        
    }

    /**
     * 
     */
    void FixedUpdate()
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
                if (!(this.checkIfWithinStoppingDistance(stoppingDistance)))
                {
                    moveTowardsTarget();
                   
                }
            }
        }

        updateVariables(); // last call
        
    }
    /**
     * @param distance
     * 
     */
    private bool checkIfWithinStoppingDistance(float distance)
    {
        float currentDistance = Vector3.Distance(target.transform.position, transform.position);
        if (currentDistance <= distance)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    /**
     * 
     */
    [ContextMenu("Activate pathfinding to target")]
    private void moveTowardsTarget()
    {
        if (target != null)
        {

            if (navMeshAgent.destination != target.transform.position)
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(attack(attackType));
            }

            Vector3 relativePos = target.transform.position - gun.bulletSpawn.transform.position; // adjust for speed
     
            if (this.checkIfWithinStoppingDistance(stoppingDistance))
            {
                canAttack = true;
                stopMovement();
            }
            else
            {
                resumeMovement();
            }
            gun.bulletSpawn.transform.rotation = Quaternion.LookRotation(relativePos);
            
            // add aim  variance

        }
        else
        {
            Debug.LogWarning(gameObject.name + " : Target Was null trying to fix in script");
            Debug.LogError(gameObject.name + " : Target is null, could NOT fix in script. Fix in inspector");
        }

    }

    /**
     * 
     */
    [ContextMenu("Stop Movement")]
    private void stopMovement()
    {
        if (navMeshAgent != null)
        {
            if (navMeshAgent.isStopped == false)
            {
                //navMeshAgent.acceleration = 0f;
                navMeshAgent.SetDestination(gameObject.transform.position);
            }
        }

    }

    /**
     * 
     */
    private void resumeMovement()
    {
        if (navMeshAgent != null)
        {
            if (enemyAttributes != null)
            {
                navMeshAgent.SetDestination(target.transform.position);
                
                // navMeshAgent.acceleration = enemyAttributes.getMovementSpeed;

            }
        }
      
    }
    /**
     * 
     */
    public IEnumerator timeOutDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

    }
    /**
     * 
     */
    private void dataValidation() // in normal programing done through interface calsses, validates data before ussage
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
                activateAnimationUssage = true;
                Debug.LogWarning("Found animator, animator is now fixed. Check inspector to see if this could be prevented next time");
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

    /**
     * Updates the variables of this script.
     * Should be last call in update method
     */
    private void updateVariables() // mostly for debug variables
    {
        entityStatusDubug = entityStatus;
        entityLocation = gameObject.transform.position;
        canMoveDebug = canMove;
        canAttackDebug = canAttack;
        interruptedActive = interrupt;
        navMeshPathfindingSpeed = navMeshAgent.speed;
    }

    /**
     * @param attackType
     * Coroutine that executes an attack dependent on the enum attayType
     */
    [ContextMenu("Activate Attack")]
    public IEnumerator attack(AIAttackType attackType)
    {
        yield return new WaitUntil(() => canAttack == true);
        StartCoroutine(executeAttack(attackType));

    }

    /**
     * 
     */
    private IEnumerator executeAttack(AIAttackType attackType) // this should call the attack animation
    {
        currentAttackExecution = attackType;
        switch (currentAttackExecution)
        {
            case AIAttackType.Gun:
                if (gun != null)
                {
                    gun.EnemyFire();
                }
                break;
            case AIAttackType.Physical:
                break;
            case AIAttackType.None:
                Debug.LogError("attempted to attack woth no weapon. This should not have happened");
                break;
          
        }



        yield return new WaitForSeconds(0f); // replace this line


        attackCoroutine = null; // this must be last call

    }


    /**
     * 
     */
    [ContextMenu("Cancel Attack")]
    private void cancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        
    }

    /**
     * 
     */
    public bool isEquippedWeaponGun()
    {
        return false;
    }
    /**
     * 
     */
    public bool isEquippedWeaponMeele()
    {
        return false;
    }
    /**
     * 
     */
    public bool hasWeapon()
    {
        if (weapon != null)
        {
            return true;
        }

        return false; // no need for else as return stops statement before reaching this
    }

    /**
     * @Param state
     * A method that querries the current state of the animator to the specified paramter
     */
    public bool isAnimatorPlayingState(string state)
    {
        const int LAYER_INDEX = 0;
        if (animator != null)
        {
            return animator.GetAnimatorTransitionInfo(LAYER_INDEX).IsName(state); //return if animation state of specified paramater is player
        }
        return false;
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
        if (other.transform.root.gameObject == target.transform.root.gameObject) // if we are doing shooting we need a seperate trigger collider on player for just range.
        {
            if (other.gameObject.tag == "RangeCollider") // this is for gun attacks
            {
                if (attackCoroutine == null) // checks to see if the ai is attacking already
                {
                    attackCoroutine = StartCoroutine(executeAttack(AIAttackType.Gun));
                }
                else if (currentAttackExecution != AIAttackType.Gun) // an attack is happening but we ovveride it with neew attack. This can happen if a player is running into the enemies physical trigger range where shooting would not be effective.
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                    attackCoroutine = StartCoroutine(executeAttack(AIAttackType.Gun));

                }
            }
            else // this is for meele attack
            {
                if (attackCoroutine == null )
                {
                    attackCoroutine = StartCoroutine(executeAttack(AIAttackType.Physical)); // starts attack 
                }
                else if (currentAttackExecution != AIAttackType.Physical) // an attack is happening but we ovveride it with neew attack. This can happen if a player is running into the enemies physical trigger range where shooting would not be effective.
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                    attackCoroutine = StartCoroutine(executeAttack(AIAttackType.Physical)); // starts attack with differenct attack type

                }

            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    //setters and getters here

}
