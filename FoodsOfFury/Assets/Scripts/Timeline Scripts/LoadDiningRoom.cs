using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDiningRoom : MonoBehaviour
{
    void OnEnable()
    {
        //SceneManager.LoadScene("DiningRoomLevel", LoadSceneMode.Single);
        StartCoroutine(Loading("DiningRoomLevel"));
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
}
