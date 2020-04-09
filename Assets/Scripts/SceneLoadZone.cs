using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadZone : MonoBehaviour
{
    public string sceneToLoad;
    bool touchingPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            touchingPlayer = true;
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
        if(touchingPlayer && Input.GetButtonDown("Interact"))
        {
            FindObjectOfType<SceneTransitions>().LoadNewScene(sceneToLoad);
        }
    }
}
