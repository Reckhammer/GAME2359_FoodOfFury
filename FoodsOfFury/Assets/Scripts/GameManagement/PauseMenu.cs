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
    public GameObject settingMenuUI;
    public GameObject objectiveTxt;     //Reference to the objective text UI
    public AudioSource LevelMusic;      //For Level Music pause and resume -Brian

    private void Start()
    {
        if ( Time.timeScale == 1.0f )
        {
            gameIsPaused = false;
        }
    }

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
        
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");
        if (LevelMusic != null)
        {
            LevelMusic?.Play(); //Level Music resumes -Brian
        }
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
        objectiveTxt.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause(bool playSound = true)
    {
        if (playSound)
        {
            // the AudioManager object carries over scenes, you can use it to carry UI audio over scenes
            AudioManager.Instance.playRandom(transform.position, "UI_Pause_01").transform.parent = AudioManager.Instance.transform;
        }
        if (LevelMusic != null)
        {
            LevelMusic.Pause(); //Level Music pauses -Brian
        }
        pauseMenuUI.SetActive(true);
        settingMenuUI.SetActive(false);
        objectiveTxt.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Settings()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);
    }

    public void BackToPause()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Back_01");

        pauseMenuUI.SetActive(true);
        settingMenuUI.SetActive(false);
    }

    public void LoadMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        StartCoroutine(Loading("MenuScene"));
    }

    private IEnumerator Loading(string level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            print("progress: " + progress);
            UIManager.instance?.setLoadingProgress(progress);
            yield return null;
        }
        gameIsPaused = false;
        Time.timeScale = 1f;
    }
}
