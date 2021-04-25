using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadKitchenLevel : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("KitchenLevel_1", LoadSceneMode.Single);
    }
}
