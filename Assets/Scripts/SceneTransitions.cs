using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator anim;
    public bool LockCursor;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if (LockCursor)
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

    public void LoadNewScene(string sceneName)
    {
        anim.SetTrigger("Fade Out");
        StartCoroutine(WaitHalfSecond(sceneName));
    }

    IEnumerator WaitHalfSecond(string sceneName)
    {
        yield return new WaitForSeconds(.5f);
        if (sceneName != "current")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
