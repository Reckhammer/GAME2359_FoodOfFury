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
        SceneManager.LoadScene("KitchenLevel");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }


}
