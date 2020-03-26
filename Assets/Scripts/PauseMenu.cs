using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool gamePaused;
    public GameObject menu;

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Pause") && !gamePaused)
        {
            PauseGame();
        }
        else if (Input.GetButtonDown("Pause") && gamePaused)
        {
            UnPauseGame();
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        menu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPauseGame()
    {
        gamePaused = false;
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
