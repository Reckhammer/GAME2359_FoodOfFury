using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageUI : MonoBehaviour
{
    public GameObject cageUI;
    //public GameObject keyUI;
    public GameObject activationObject;
    private bool showUI = true;
    private Objective objComponent;     //Objective component of activation object
    private bool isObjective = false;   //boolean indicating if the activation object is an objective

    // Start is called before the first frame update
    void Start()
    {
        cageUI.SetActive(false);
        //keyUI.SetActive(false);

        //Check if activationObject is an objective
        objComponent = activationObject.GetComponent<Objective>();

        //If there is an objective component
        //      then it is an objective
        if (objComponent != null)
        {
            isObjective = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            cageUI.SetActive(false);
        }

        /*if (objComponent.needKey == true)
        {
            keyUI.SetActive(true);
            cageUI.SetActive(false);
        }
        else
        {
            keyUI.SetActive(false);
        }*/

        //If it is an objective AND it is done AND showUI is true
        //      Disable the popup
        if (isObjective && objComponent.isDone && showUI)
        {
            showUI = false;
            //keyUI.SetActive(false);
            cageUI.SetActive(false);
        }
        else if (activationObject.activeSelf == false && showUI == true)
        {
            showUI = false;
            //keyUI.SetActive(false);
            cageUI.SetActive(false);
        }


    }




}
