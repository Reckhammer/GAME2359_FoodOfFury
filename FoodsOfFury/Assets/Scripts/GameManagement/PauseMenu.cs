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

        AudioManager.Instance.playRandom(transform.position, "UI_Back_01");

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        // the AudioManager object carries over scenes, you can use it to carry UI audio over scenes
        AudioManager.Instance.playRandom(transform.position, "UI_Pause_01", "UI_Pause_02").transform.parent = AudioManager.Instance.transform;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

}
