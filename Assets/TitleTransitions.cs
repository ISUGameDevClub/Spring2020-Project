using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleTransitions : MonoBehaviour
{
    public Animator anim;
    public string sceneName;

    public void FadeOut()
    {
        anim.SetTrigger("Fade Out");
        StartCoroutine(WaitHalfSecond());
    }

    IEnumerator WaitHalfSecond()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }

}
