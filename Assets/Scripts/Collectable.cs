using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int collectable;

    private void Start()
    {
        if(Collectables.collectables[collectable] == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collectables.amountCollected++;
            Collectables.collectables[collectable] = true;
            if(Collectables.amountCollected >= 4)
            {
                //"curret" reloads scene you are in
                FindObjectOfType<SceneTransitions>().LoadNewScene("VictoryScreen");
            }
            Destroy(gameObject);
        }
    }

}
