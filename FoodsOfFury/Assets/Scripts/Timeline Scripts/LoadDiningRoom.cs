using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDiningRoom : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("DiningRoomLevel", LoadSceneMode.Single);
    }
}
