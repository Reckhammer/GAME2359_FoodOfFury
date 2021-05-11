using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{

    public GameObject popupUI;
    public GameObject activationObject;
    private bool showUI = true;
    private Objective objComponent;     //Objective component of activation object
    private bool isObjective = false;   //boolean indicating if the activation object is an objective


    // Start is called before the first frame update
    void Start()
    {
        popupUI.SetActive(false);

        //Check if activationObject is an objective
        objComponent = activationObject.GetComponent<Objective>();

        //If there is an objective component
        //      then it is an objective
        if ( objComponent != null )
        {
            isObjective = true;
        }
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            popupUI.SetActive(false);
        }

        //If it is an objective AND it is done AND showUI is true
        //      Disable the popup
        if ( isObjective && objComponent.isDone && showUI )
        {
            showUI = false;
            popupUI.SetActive(false);
        }
        else if (activationObject.activeSelf == false && showUI == true)
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
