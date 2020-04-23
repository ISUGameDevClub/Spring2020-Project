using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@Author Jake Botka - Programming Team
public static class Gun_Utils
{
    //@Author Jake Botka - Programming Team

    public static Quaternion aimVariance(int Difficulty, GameObject origin, GameObject target)
    {
        Quaternion Variance = Quaternion.LookRotation(target.transform.position - origin.transform.position);
        // add vairance;
        return Variance;
    }


}
