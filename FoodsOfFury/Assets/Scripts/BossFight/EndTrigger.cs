using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    private Coroutine transition;

    private void OnTriggerEnter(Collider other)
    {
        if (transition == null && other.tag == "Player")
        {
            transition = StartCoroutine(doTransition());
        }
    }

    private IEnumerator doTransition()
    {
        AudioManager.Instance.playRandom(transform.position, "Rollo_Win_01", "Rollo_Win_02");
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0.0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync("Cutscene_Ending");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            print("progress: " + progress);
            UIManager.instance?.setLoadingProgress(progress);
            yield return null;
        }
    }
}
