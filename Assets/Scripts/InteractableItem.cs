﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public int powerUp;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (powerUp == 1)
                other.GetComponent<PlayerController>().lowGravity = true;
            else if (powerUp == 2)
                other.GetComponent<PlayerController>().SpeedBoost = true;
            else if (powerUp == 3)
                other.GetComponent<PlayerController>().doubleJump = true;

            Destroy(gameObject);
        }
    }
}
