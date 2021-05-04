using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    private Coroutine transition;

    private void OnTriggerEnter(Collider other)
    {
        if (transition == null)
        {
            transition = StartCoroutine(doTransition());
        }
    }

    private IEnumerator doTransition()
    {
        AudioManager.Instance.playRandom(transform.position, "Rollo_Win_01", "Rollo_Win_02");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Cutscene_Ending");
    }
}
