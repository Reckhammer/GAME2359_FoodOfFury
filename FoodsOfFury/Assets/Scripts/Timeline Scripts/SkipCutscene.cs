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
        StartCoroutine(Loading(index));
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
        UIManager.instance?.setLoadingProgress(Mathf.Clamp01(operation.progress / 0.9f));
    }
}
