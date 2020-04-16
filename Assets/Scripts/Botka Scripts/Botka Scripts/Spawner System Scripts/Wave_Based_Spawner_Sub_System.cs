using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Based_Spawner_Sub_System : MonoBehaviour
{
    public GameObject[] entityPrefabs;
    [Tooltip("Each slot is its own wave, seperate enemy type identification numbers with a comma each")]
    public string[] waveSequence;

    [Header("Debug")]
    public string[] waveSequenceDebug;
    // Start is called before the first frame update
    void Start()
    {
        if (waveSequence != null)
        {
            waveSequenceDebug = new string[waveSequence.Length];
        }
        else
        {
            waveSequenceDebug = new string[1];

        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }


    public void translateWaveSequence(int index)
    {
        if (waveSequence != null)
        {
            if (waveSequence.Length > 0)
            {
                string seq = waveSequence[index];
                char[] x = seq.ToCharArray();
                int index1 = 0;
                
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i].ToString().Equals(","))
                    {
                        

                    }
                    else
                    {
    
                        int num = int.Parse(x.ToString());
                            waveSequenceDebug[index1] += num.ToString();

                    }

                    index1++;
                }

            }
        }
        else
        {
            // debug error send
        }
    
    }
}
