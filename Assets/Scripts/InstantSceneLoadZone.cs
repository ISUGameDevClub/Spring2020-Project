using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSceneLoadZone : MonoBehaviour
{
    public string sceneToLoad;
    bool touchingPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = true;
        }
    }

    private void Update()
    {
        if (touchingPlayer)
        {
            FindObjectOfType<SceneTransitions>().LoadNewScene(sceneToLoad);
        }
    }
}
