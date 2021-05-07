using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{

    public GameObject popupUI;
    private float turnOffUI = 4.0f;
    //public GameObject nullObject;


    // Start is called before the first frame update
    void Start()
    {
        popupUI.SetActive(false);
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            popupUI.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            popupUI.SetActive(true);

            StartCoroutine(UITimer());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            popupUI.SetActive(false);
        }
    }

    private IEnumerator UITimer()
    {
        float passed = 0.0f;

        while (passed < turnOffUI)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        popupUI.SetActive(false);
    }



}
