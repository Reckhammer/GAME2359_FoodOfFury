using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class represents a simple health amount for an entity.
//----------------------------------------------------------------------------------------

public class nHealth : MonoBehaviour
{
    public float max = 0.0f;                    // max amount of health
    public float amount { get; private set; }   // current amount of health
    public delegate void Update(float amount);  // delegate used with  OnUpdate
    public event Update OnUpdate;               // event that sends a message when amount updates

    private bool isInvincible = false;          // invincibility boolean

    private void Start()
    {
        amount = max;
    }

    // adds to the health given a value normalized
    public bool add(float value)
    {
        if (amount == max || amount == 0) // don't set if full or dead
        {
            return false;
        }

        // sum = amount + (value normalized to a positive)
        float sum = (value >= 0.0f) ? amount + value : amount + -value;

        // sum is above max, set to max
        if (sum > max)
        {
            amount = max;
        }
        else // set amount to sum
        {
            amount = sum;
        }

        if (OnUpdate != null)
        {
            OnUpdate(amount); // send update message
        }

        return true;
    }

    // subtracts from the health given a value normalized
    public void subtract(float value)
    {
        if (isInvincible || amount == 0.0f) // return if invincible or 'dead'
        {
            return;
        }

        // difference = amount - (value normalized to a positive)
        float difference = (value >= 0.0f) ? amount - value : amount - -value;

        // difference is below 0, set amount to 0
        if (difference < 0)
        {
            amount = 0;
        }
        else // set amount to difference
        {
            amount = difference;
        }

        if (OnUpdate != null)
        {
            OnUpdate(amount); // send update message
        }
    }

    // gives entity invincibility time (health can no longer be subtracted)
    public void giveInvincibility(float time = 0.0f)
    {
        if (time > 0.0f) // don't start timer if not needed
        {
            StartCoroutine(CountInvincibility(time));
        }
    }

    public void revive()
    {
        if (OnUpdate != null)
        {
            amount = max;

            OnUpdate(amount);
        }
    }

    // function to act as a kill (to get around invincibility)
    public void deplete()
    {
        amount = 0.0f;

        if (OnUpdate != null)
        {
            OnUpdate(amount); // send update message
        }
    }

    // this counts the invincibility timer
    public IEnumerator CountInvincibility(float time)
    {
        isInvincible = true;
        float passed = 0.0f;

        // count up the passed until we reach time
        while (passed < time)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        isInvincible = false;
    }

    // returns isInvincible
    public bool isNowInvincible()
    {
        return isInvincible;
    }
}
