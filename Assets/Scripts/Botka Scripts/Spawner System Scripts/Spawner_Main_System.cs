using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Main_System : MonoBehaviour
{
    public enum SpawnerType
    {
        SingleShotSpawn, TimeRepeatingSpawn, WaveBasedSpawn

    }


    public SpawnerType spawnerSystemType;

    public Activation_Spawner_Sub_System activationSpawnerSubSystem;
    public Time_Repeating_Spawner_Sub_System timeRepeatingSubSystem;
    public Wave_Based_Spawner_Sub_System waveBasedSystem;


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



    }


}
