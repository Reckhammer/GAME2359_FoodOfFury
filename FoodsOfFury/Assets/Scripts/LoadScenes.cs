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

    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("Quit game");
        }

    }



}
