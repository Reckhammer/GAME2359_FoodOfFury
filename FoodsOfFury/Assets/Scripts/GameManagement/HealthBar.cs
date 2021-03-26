using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Manages a health bar UI object
//----------------------------------------------------------------------------------------

public class HealthBar : MonoBehaviour
{
    public Image fill;              // fill imgage reference (used with gradient)
    //public Text text;             // text box showing amount (debug)

    private float maxValue = 0.0f;

    // sets the max value of slider
    public void setHealthBarMax(float max)
    {
        maxValue = max;
    }

    // updates health bar (slider, text, fill color)
    public void updateHealthBar(float amount)
    {
        fill.fillAmount = amount / maxValue;
        //text.text = amount.ToString();
    }
}