using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialStart : MonoBehaviour
{
    private TimeTrial tt;

    private void Start()
    {
        tt = GetComponentInParent<TimeTrial>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            tt.StartTrial();
        }
    }
}
