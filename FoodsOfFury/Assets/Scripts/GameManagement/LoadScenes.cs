using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//----------------------------------------------------------------------------------------
// Author(s): Abdon J. Puente IV
//
// Description: This class manages the games' scenes
//----------------------------------------------------------------------------------------

public class LoadScenes : MonoBehaviour
{

    public void FreezerScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("Cutscene_Freezer"));
        //SceneManager.LoadScene("Cutscene_Freezer");
    }

    public void KitchenScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("Cutscene_Kitchen"));
        //SceneManager.LoadScene("Cutscene_Kitchen");
    }

    public void DiningScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("Cutscene_DiningRoomFlyover"));
        //SceneManager.LoadScene("Cutscene_DiningRoomFlyover");
    }

    public void ThroneRoom()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("ThroneRoom"));
        //SceneManager.LoadScene("ThroneRoom");
    }

    public void LevelSelect()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("LevelSelect"));
        //SceneManager.LoadScene("LevelSelect");
    }

    public void MainMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("MenuScene"));
        //SceneManager.LoadScene("MenuScene");
    }

    public void SettingsMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("SettingsScene"));
        //SceneManager.LoadScene("SettingsScene");
    }

    public void HowToPlay()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("HowToPlay"));
        //SceneManager.LoadScene("HowToPlay");
    }

    public void Credits()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading("CreditsScene"));
        //SceneManager.LoadScene("CreditsScene");
    }

    public void RestartScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;

        StartCoroutine(Loading(SceneManager.GetActiveScene().name));
        //SceneManager.LoadScene( SceneManager.GetActiveScene().name ); //load the current scene
    }

    public void NextScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01").transform.parent = AudioManager.Instance.transform;
        int index = (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings) ? SceneManager.GetActiveScene().buildIndex + 1 : 0;

        StartCoroutine(Loading(index)); // Load the next scene in the build order
        //SceneManager.LoadScene(index); // Load the next scene in the build order
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void SurveyLink()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLScCxpZfb3biGQN74ZbQQoxYeUpvrhDm9DYNWUAnRekP8nnLcA/viewform");
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
    }

    private IEnumerator Loading(int level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            print("progress: " + progress);
            UIManager.instance?.setLoadingProgress(progress);
            yield return null;
        }
    }
}
