using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
* @Author Jake Botka
* 
*/
public class AiManager : MonoBehaviour
{
    public const string TAG = "AiManager";
    [SerializeField]private int numberOfActiveAITargetingPLayer;
    [SerializeField] [Range(25f, 500f)] private int rangeForAiActivation;
    [Header("Debug - DO NOT SET")]
    [SerializeField] private int currentNumberOfActiveAI;
    [SerializeField]private int currentNumberOfAITargetingPlayer;
    [SerializeField]private List<GameObject> allAI;
    [SerializeField]private List<GameObject> allActiveAI;


    private GameObject player;
    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start()
    {
        allAI = new List<GameObject>();
        allActiveAI = new List<GameObject>();
        currentNumberOfActiveAI = 0;
        currentNumberOfAITargetingPlayer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.tag = TAG;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allAI.Count > 0)
        {
            this.activateAiInRange();
        }
    }

    public void activateAiInRange()
    {
        if (allAI.Count > 0)
        {
            for (int i = 0; i < allAI.Count; i++)
            {
                if (allActiveAI.Count >= 0)
                {
                    float newRangeActivation = rangeForAiActivation;
                    GameObject obj = allAI[i];
                    AI script = obj.GetComponentInChildren<AI>();
                    if (script.customActivationRangeOverride)
                    {
                        newRangeActivation = script.activationRange;
                    }
                    

                    if (Vector3.Distance(player.transform.position, obj.transform.position) <= newRangeActivation)
                    {
                        if (allActiveAI.Contains(obj) == false)
                        {
                            this.activateAI(obj);
                        }
                    }
                    else
                    {
                        if (allActiveAI.Contains(obj))
                        {
                            this.deactivateAI(obj);
                        }

                    }
                }
            }

        }

    }


    public void registerAI(GameObject parent)
    {
        if (allAI.Contains(parent) == false)
        {
            allAI.Add(parent);
        }
    }

    public void unRegisterAI(GameObject parent)
    {
        if (allAI.Contains(parent))
        {
            allAI.Remove(parent);
        }
    }

    public void activateAI(GameObject AI)
    {
        if (allActiveAI.Contains(AI) == false)
        {
            AI.GetComponentInChildren<AI>().enabled = true;
            allActiveAI.Add(AI);
        }
    }

    public void deactivateAI(GameObject AI)
    {
        if (allActiveAI.Contains(AI))
        {
            AI.GetComponentInChildren<AI>().enabled = false;
            allActiveAI.Remove(AI);
        }
    }
}
