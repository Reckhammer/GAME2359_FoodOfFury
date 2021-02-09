using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class handles the "damaging" of an objects 'health' (class)
//              when it hits this objects trigger.
//----------------------------------------------------------------------------------------

public class Damaging : MonoBehaviour
{
    public string[] targets;            // targets to damage
    public float damageAmount = 0.0f;   // amount of damage to be delt
    public float delayAmount  = 0.0f;   // time delay to be able damage again

    private void OnTriggerEnter(Collider other)
    {
        if (targets.Length == 0)
        {
            print(name + " does not have targets set up!");
            return;
        }

        // iterate through targets and compare with 'other.tag'
        foreach (string target in targets)
        {
            if (other.tag == target)
            {
                other.GetComponent<Health>().subtract(damageAmount, delayAmount); // subtract from other's 'health' and add delay
            }
        }
    }
}
