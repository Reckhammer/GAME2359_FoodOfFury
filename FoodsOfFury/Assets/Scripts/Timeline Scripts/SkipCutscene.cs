using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
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
