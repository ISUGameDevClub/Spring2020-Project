using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @Author Jake Botka - Programming Team

public class Health : MonoBehaviour
{
    // @Author Jake Botka - Programming Team
    public enum ParentType
    {
        Player, Enemy, Object, None
    }

    [HideInInspector] public const float DefaultMaxHealth = 100f; // constant variable

    [SerializeField] private bool turnOnDebugToConsole;


    [Tooltip("This is the max health of the gameobject Entity")]
    public float maxHealth;
    [Tooltip("This is set true if you want to disable destroy functions on health reaching 0")]
    public bool OverideDestroyFunctions;

    //[HideInInspector] public Player player; // player script if it exists
    //[HideInInspector] public Enemy enemy; // enemy script if it exists


    [Header("Debug - DO NOT SET")]
    public ParentType parentType; // this shows the type of parent
    public float currentFillPercent; // fill pereent for healthbar UI;
    public float currentHealth; // current health of entity

    public Coroutine x;

    void Awake() // this is called before first active frame
    {
        // HealthBar = GameObject.FindObjectOfType<HealthBAr>(); //finds and loads active script in scene. Uncomment when healthbar script is done
    }
    void Start() // called first active frame after awake
    {
        determineParent();
        if (currentHealth <= 0f)
        {
            currentHealth = maxHealth;

        }

        if (maxHealth <= 0f)
        {
            maxHealth = Health.DefaultMaxHealth; //calls the script constant variable which has to be called statically
        }







    }
    // Update is called once per frame
    void FixedUpdate()
    {
        dataValidation();
        if (parentType != ParentType.None) // ensures that correct scripts are in place before handing health functions
        {
            checkHealth();
            handleGui();
        }

        if (turnOnDebugToConsole) // option for game designer to have all debug messaages to be sent to the console on each update
        {
            debugLogMessages();
        }



    }



    private void determineParent()
    {
        GameObject parent = gameObject.transform.root.gameObject; // find the upmoast parent of the gameobject
        /*
        if (parent.GetComponentInChildren<Player>() != null)
        {
            parentType = ParentType.Player;
            player = parent.GetComponentInChildren<Player>();

        }
        else if (parent.GetComponentInChildren<Enemy>() != null)
        {
            parentType = ParentType.Enemy;
            enemy = parent.GetComponentInChildren<Enemy>();
        }
        else
        {
            parentType = ParentType.None;
        }
        */




    }


    public void checkHealth()
    {
        if (currentHealth <= 0f && OverideDestroyFunctions != true) // if health is zero and override is false then destroy
        {
            if (gameObject.tag == "Player")
            {
                FindObjectOfType<SceneTransitions>().LoadNewScene("current");
            }
            else
            {
                Debug.LogWarning(gameObject.name + "Has Died"); // logs death and deathorigin
                Destroy(gameObject.transform.root.gameObject); // Destroys gameobject
            }

        }

      


    }


    private void handleGui()
    {
        currentFillPercent = currentHealth / maxHealth;
        //greenHealthBar.fillAmount = currentFillPercent;

        // later will call the health bar script 

    }



    public void recieveDamage(float damage) // this should be called either directly or via gameobject.sendMessage("recieveDamage", float damage) from external script;
    {
        currentHealth -= damage;
        if (currentHealth <= 0f && OverideDestroyFunctions != true) // if health is zero and override is false then destroy
        {
            if(gameObject.tag == "Player")
            {
                FindObjectOfType<SceneTransitions>().LoadNewScene("current");
            }
            else
            {
                Debug.LogWarning(gameObject.name + "Has Died"); // logs death and deathorigin
                Destroy(gameObject.transform.root.gameObject); // Destroys gameobject
            }

        }

    }




    private void dataValidation()
    {
        if (maxHealth <= 0f) // this detects if the game designer did not assign the entityes max health
        {
            maxHealth = Health.DefaultMaxHealth; //calls the script constant variable which has to be called statically
            Debug.LogWarning("Max health not assigned in inspector. Default values used");
        }

        if (parentType == ParentType.None) // this means that the health script is attached to an inncorect gameobject or the gammeobject is missing a player or enemy script
        {
            Debug.LogError("Health script missing parent script");
        }
    }
    private void OnDestroy() // is called on destroy
    {

        Debug.LogWarning(gameObject.transform.root.name + "was Destroyed");



    }

    private void debugLogMessages()
    {

        Debug.Log(gameObject.name + " current health is : " + currentHealth);

    }


}

