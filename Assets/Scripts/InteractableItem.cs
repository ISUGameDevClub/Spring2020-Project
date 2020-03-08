using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public int powerUp;

    private bool active = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && active)
        {
            if (powerUp == 0)
            {
                other.GetComponent<PlayerController>().lowGravity = false;
                other.GetComponent<PlayerController>().SpeedBoost = false;
                other.GetComponent<PlayerController>().doubleJump = false;
                other.GetComponent<PlayerController>().slowTime = false;
            }
            else if (powerUp == 1)
                other.GetComponent<PlayerController>().lowGravity = true;
            else if (powerUp == 2)
                other.GetComponent<PlayerController>().SpeedBoost = true;
            else if (powerUp == 3)
                other.GetComponent<PlayerController>().doubleJump = true;
            else if (powerUp == 4)
                other.GetComponent<PlayerController>().slowTime = true;

            active = false;
            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
