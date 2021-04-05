using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{

    public void FreezerScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        SceneManager.LoadScene("FreezerLevel");
    }

    public void KitchenScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        SceneManager.LoadScene("KitchenLevel_1");
    }

    public void MainMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        SceneManager.LoadScene("MenuScene");
    }

    public void SettingsMenu()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        SceneManager.LoadScene("SettingsScene");
    }

    public void HowToPlay()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        SceneManager.LoadScene("HowToPlay");
    }

    public void Credits()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

        print("credits");
        //SceneManager.LoadScene("Credits");
    }

    public void RestartScene()
    {
        AudioManager.Instance.playRandom(transform.position, "UI_Accept_01");

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
