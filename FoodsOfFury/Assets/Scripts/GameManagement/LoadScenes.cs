using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{

    public void FreezerScene()
    {
        SceneManager.LoadScene("FreezerLevel");
    }

    public void KitchenScene()
    {
        SceneManager.LoadScene("KitchenLevel_1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name ); //load the current scene
    }

    public void NextScene()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 ); //Load the next scene in the build order
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void SurveyLink()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfrqRxvlSFzdXtY8vt_uPi_zdYZaOyhqvI2GoTRFSkB1bFvaw/viewform");
    }
}
