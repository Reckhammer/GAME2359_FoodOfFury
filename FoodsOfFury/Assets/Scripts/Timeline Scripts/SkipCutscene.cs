using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Skip();
        }
    }

    void Skip()
    {
        int index = (SceneManager.GetActiveScene().buildIndex + 1 != SceneManager.sceneCountInBuildSettings) ? SceneManager.GetActiveScene().buildIndex + 1 : 0;
        SceneManager.LoadScene(index);
    }



}
