using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: A simple health construct that manages an amount of health
//
// TODO: set time limits to when amount can be changed negatively (i.e. invisibility time)
//----------------------------------------------------------------------------------------

public class Health : MonoBehaviour
{
    public float max = 0.0f;                    // max amount of health
    public float amount { get; private set; }   // current amount of health

    private void Start()
    {
        amount = max;
    }

    // adds to the health given a value normalized
    public void add(float value)
    {
        // sum = amount + (value normalized to a positive)
        float sum = (value >= 0) ? amount + value : amount + -value;

        // sum is above max, set to max
        if (sum > max)
        {
            amount = max;
        }
        else // set amount to sum
        {
            amount = sum;
        }
    }

    // subtracts from the health given a value normalized
    public void subtract(float value)
    {
        // difference = amount - (value normalized to a positive)
        float difference = (value >= 0) ? amount - value : amount - -value;

        // difference is below 0, set amount to 0
        if (difference < 0)
        {
            amount = 0;
        }
        else // set amount to difference
        {
            amount = difference;
        }
    }
}
