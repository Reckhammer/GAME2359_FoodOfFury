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

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.OpenURL("https://docs.google.com/forms/d/1i7uiVFnOXbPAyJPPxXvLberbDucR8U2HRrbW6Xsxqi0/viewform?edit_requested=true");
        Application.Quit();
    }


}
