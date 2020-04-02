using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleTransitions : MonoBehaviour
{
    public Animator anim;
    public string sceneName;
    public bool LockCursor;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if(LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

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

    public void QuitGame()
    {
        Application.Quit();
    }
}
