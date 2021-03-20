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
    public Slider slider;       // slider component that manages fill image
    public Image fill;          // fill imgage reference (used with gradient)
    public Gradient gradient;   // gradient to be used (temp?)
    public Text text;           // text box showing amount (debug)

    // sets the max value of slider
    public void setHealthBarMax(float max)
    {
        slider.maxValue = max;
    }

    // updates health bar (slider, text, fill color)
    public void updateHealthBar(float amount)
    {
        slider.value = amount;
        text.text = amount.ToString();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}