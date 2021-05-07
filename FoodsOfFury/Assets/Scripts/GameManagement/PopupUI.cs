using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{

    public GameObject popupUI;
    public GameObject activationObject;
    private bool showUI = true;


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

        if (activationObject.activeSelf == false && showUI == true)
        {
            showUI = false;
            popupUI.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && showUI == true)
        {

            popupUI.SetActive(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            popupUI.SetActive(false);
        }
    }


}
