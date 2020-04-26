using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGoo : MonoBehaviour
{
    public int damage;
    private bool touchingPlayer;
    private Health playerHealth;
    private PlayerController pc;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        playerHealth = pc.gameObject.GetComponent<Health>();
        StartCoroutine(InGoo());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = true;
            pc.forcedWalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = false;
            pc.forcedWalk = false;
        }
    }

    IEnumerator InGoo()
    {
        while (true)
        {
            if (touchingPlayer)
            {
                playerHealth.recieveDamage(damage);
                yield return new WaitForSeconds(.1f);
            }
            else
            {
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
