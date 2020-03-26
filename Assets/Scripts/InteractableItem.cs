using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public int powerUp;
    public float PowerUpLength;

    private bool active = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && active)
        {
            if (powerUp == 0)
            {
                other.GetComponent<PlayerController>().lowGravity = 0;
                other.GetComponent<PlayerController>().SpeedBoost = 0;
                other.GetComponent<PlayerController>().doubleJump = 0;
                other.GetComponent<PlayerController>().slowTime = 0;
            }
            else if (powerUp == 1)
                other.GetComponent<PlayerController>().lowGravity = PowerUpLength;
            else if (powerUp == 2)
                other.GetComponent<PlayerController>().SpeedBoost = PowerUpLength;
            else if (powerUp == 3)
                other.GetComponent<PlayerController>().doubleJump = PowerUpLength;
            else if (powerUp == 4)
                other.GetComponent<PlayerController>().slowTime = PowerUpLength;

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
