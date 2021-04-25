using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFreezerLevel : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("FreezerLevel", LoadSceneMode.Single);
    }
}
