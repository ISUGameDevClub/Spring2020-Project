using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookDetector : MonoBehaviour
{
    public GameObject player;
    private Coroutine x;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
        {
            if (x == null)
            {
                x = StartCoroutine(delay());
                player.GetComponent<GrapplingHook>().HookReleaseWrapper();
                player.GetComponent<GrapplingHook>().hooked = true;
                player.GetComponent<GrapplingHook>().setHookedObj(other.gameObject);
            }

        }
    }

    public IEnumerator delay() {
        yield return new WaitForSeconds(.75f);
        yield return new WaitForEndOfFrame();
        x = null;
    }

}
