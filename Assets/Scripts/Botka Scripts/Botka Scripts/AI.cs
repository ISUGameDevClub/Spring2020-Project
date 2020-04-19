using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/**
 * @Author Jake Botka
 * 
 */

[RequireComponent(typeof(Enemy), typeof(Collider))]
public class AI : MonoBehaviour
{
    /**
     * @Author Jake Botka
     * 
     */
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
    public float stoppingDistance;

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
        CanAttack = false;
        CanMove = false;
        Interrupt = false;
        
        
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
        CanMove = true;

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

        if (Interrupt)
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }

            // stop movement
   
        }

        if (CanMove)
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
    * 
    */
    private void dataValidation() // in normal programing done through interface calsses, validates data before ussage
    {
        if (target == null)
        {
            // handle
        }

        if (CanAttack == false)
        {
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }

        if (animator == null) // animator variable is null, lets chekc to see if it exists
        {
            ActivateAnimationUssage = false;
            Debug.LogWarning("Animator is missing attemting to fix in script");

            if (gameObject.GetComponent<Animator>() != null) // checks its existance
            {
                animator = gameObject.GetComponent<Animator>();
                ActivateAnimationUssage = true;
                Debug.LogWarning("Found animator, animator is now fixed. Check inspector to see if this could be prevented next time");
            }
            else
            {
                Debug.LogError("Animator is missing, could NOT fix in script");
            }
        }
        else // animator is not null and exists
        {
            ActivateAnimationUssage = true;
        }

    }

    /**
     * Updates the variables of this script.
     * Should be last call in update method
     */
    private void updateVariables() // mostly for debug variables
    {
        entityStatusDubug = EntityStatus1;
        entityLocation = gameObject.transform.position;
        canMoveDebug = CanMove;
        canAttackDebug = CanAttack;
        interruptedActive = Interrupt;
        navMeshPathfindingSpeed = navMeshAgent.speed;
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
            if (AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(attack(attackType));
            }

            Vector3 relativePos = target.transform.position - gun.bulletSpawn.transform.position; // adjust for speed
     
            if (this.checkIfWithinStoppingDistance(stoppingDistance))
            {
                CanAttack = true;
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
     * @param attackType
     * Coroutine that executes an attack dependent on the enum attayType
     */
    [ContextMenu("Activate Attack")]
    public IEnumerator attack(AIAttackType attackType)
    {
        yield return new WaitUntil(() => CanAttack == true);
        StartCoroutine(executeAttack(attackType));

    }

    /**
     * @param attackType
     * Exeuctes attack using a couroutine that is dependent on the return value of @param.
     */
    private IEnumerator executeAttack(AIAttackType attackType) // this should call the attack animation
    {
        CurrentAttackExecution = attackType;
        switch (CurrentAttackExecution)
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


        AttackCoroutine = null; // this must be last call

    }


    /**
     * Stopts the coroutine if the cororutine is active using its VARIABLE and not its method name
     */
    [ContextMenu("Cancel Attack")]
    private void cancelAttack()
    {
        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = null;
        }
        
    }

    /**
     * 
     */
    public bool isEquippedWeaponGun()
    {
        if (gun != null && attackType == AIAttackType.Gun)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    /**
     * Automatically called by unity when the lifecycle of the gameobject has ended
     */
    private void OnDestroy()
    {
        StopAllCoroutines();
        Debug.Log(gameObject.name + " was destroyed");
        
    }

    /**
     * @param other
     * Automaticalled called by unity when the trigger collider associated witht he gameobject that this instance has been attached to has triggered called this enter state method
     */
    private void OnTriggerEnter(Collider other)
    {
        // this makes sure that if the target and the target objects collider are on different hiearchy levels that it still will return true 
        //by getting the upmost parent and then comparing
        if (other.transform.root.gameObject == target.transform.root.gameObject) // if we are doing shooting we need a seperate trigger collider on player for just range.
        {
            if (other.gameObject.tag == "RangeCollider") // this is for gun attacks
            {
                if (AttackCoroutine == null) // checks to see if the ai is attacking already
                {
                    AttackCoroutine = StartCoroutine(executeAttack(AIAttackType.Gun));
                }
                else if (CurrentAttackExecution != AIAttackType.Gun) // an attack is happening but we ovveride it with neew attack. This can happen if a player is running into the enemies physical trigger range where shooting would not be effective.
                {
                    StopCoroutine(AttackCoroutine);
                    AttackCoroutine = null;
                    AttackCoroutine = StartCoroutine(executeAttack(AIAttackType.Gun));

                }
            }
            else // this is for meele attack
            {
                if (AttackCoroutine == null )
                {
                    AttackCoroutine = StartCoroutine(executeAttack(AIAttackType.Physical)); // starts attack 
                }
                else if (CurrentAttackExecution != AIAttackType.Physical) // an attack is happening but we ovveride it with neew attack. This can happen if a player is running into the enemies physical trigger range where shooting would not be effective.
                {
                    StopCoroutine(AttackCoroutine);
                    AttackCoroutine = null;
                    AttackCoroutine = StartCoroutine(executeAttack(AIAttackType.Physical)); // starts attack with differenct attack type

                }

            }

        }
    }
    /**
     * 
     * 
     */
    private void OnTriggerExit(Collider other)
    {
        
    }

    //setters and getters here

    public EntityStatus EntityStatus1 { get => entityStatus; set => entityStatus = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public bool Interrupt { get => interrupt; set => interrupt = value; }
    public bool ActivateAnimationUssage { get => activateAnimationUssage; set => activateAnimationUssage = value; }
    public AIAttackType CurrentAttackExecution { get => currentAttackExecution; set => currentAttackExecution = value; }
    public Coroutine AttackCoroutine { get => attackCoroutine; set => attackCoroutine = value; }
    public float VelocityModiferier { get => velocityModiferier; set => velocityModiferier = value; }
    public float DetectionRange { get => detectionRange; set => detectionRange = value; }

    public static string ANIMATOR_STATE_IDLE1 => ANIMATOR_STATE_IDLE;

    public static string ANIMATOR_STATE_WALKING1 => ANIMATOR_STATE_WALKING;

    public static string ANIMATOR_STATE_RUNNING1 => ANIMATOR_STATE_RUNNING;

    public static string ANIMATOR_STATE_MEELE_ONE1 => ANIMATOR_STATE_MEELE_ONE;

    public static string ANIMATOR_STATE_MEELE_TWO1 => ANIMATOR_STATE_MEELE_TWO;

    public static string ANIMATOR_STATE_MEELE_THREE1 => ANIMATOR_STATE_MEELE_THREE;

    public static string ANIMATOR_STATE_GUN_ONE1 => ANIMATOR_STATE_GUN_ONE;

    public static string ANIMATOR_STATE_GUN_TWO1 => ANIMATOR_STATE_GUN_TWO;

    public static string ANIMATOR_STATE_GUN_THREE1 => ANIMATOR_STATE_GUN_THREE;
}
