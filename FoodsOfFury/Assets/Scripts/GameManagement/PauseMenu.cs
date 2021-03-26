using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV
//
// Description: This class handles the pause menu
//----------------------------------------------------------------------------------------

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {

                Pause();
           
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        //AudioManager.Instance.play(pauseMenuUI.transform.position, "UI_Pause_01").transform.SetParent(transform);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfrqRxvlSFzdXtY8vt_uPi_zdYZaOyhqvI2GoTRFSkB1bFvaw/viewform");
        Application.Quit();
    }

}
