using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Main_System : MonoBehaviour
{
    public enum SpawnerType
    {
        SingleShotSpawn, TimeRepeatingSpawn, WaveBasedSpawn

    }


    [SerializeField] private SpawnerType spawnerSystemType;

    [Header("Scripts for sub Systems - select Only One")]
    [Header("Must match system type enum")]
    [SerializeField] private Activation_Spawner_Sub_System activationSpawnerSubSystem;
    [SerializeField] private Time_Repeating_Spawner_Sub_System timeRepeatingSubSystem;
    [SerializeField] private Wave_Based_Spawner_Sub_System waveBasedSystem;


    [Header("Debug")]
    [SerializeField] private bool error;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dataValidation();
        
    }


    public void dataValidation()
    {

        if (spawnerSystemType == SpawnerType.SingleShotSpawn)
        {
            if (activationSpawnerSubSystem != null)
            {

            }
            else
            {
                Debug.LogError("The Subsystem variable is null");
            }
        }
        else if (spawnerSystemType == SpawnerType.TimeRepeatingSpawn)
        {
            if (timeRepeatingSubSystem != null)
            {
                
            }
            else
            {
                Debug.LogError("The Subsystem variable is null");
            }

        }
        else if (spawnerSystemType == SpawnerType.WaveBasedSpawn)
        {
            if (waveBasedSystem != null)
            {

            }
            else
            {
                Debug.LogError("The Subsystem variable is null");
            }

        }
        else
        {
            Debug.LogError("You must assign a system type");
        }





    }


}
