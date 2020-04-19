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
            Collectables.collectables[collectable] = true;
            Destroy(gameObject);
        }
    }

}
