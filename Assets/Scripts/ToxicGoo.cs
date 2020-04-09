using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGoo : MonoBehaviour
{
    public int damage;
    bool touchingPlayer;
    private Health playerHealth;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = true;
            other.gameObject.GetComponent<Health>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = false;
        }
    }

    private void Update()
    {
        if (touchingPlayer)
        {
            playerHealth()
        }
    }
}
