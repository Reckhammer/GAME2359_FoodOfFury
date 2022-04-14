using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class handles the "damaging" of an objects 'health' (class)
//              when it hits this objects trigger. Like old one, but remembers objects that were hit
//              NOTE: needs to be reviewed
//----------------------------------------------------------------------------------------

public class nDamaging: MonoBehaviour
{
    public string[] targets;                            // targets to damage
    public float damageAmount = 0.0f;                   // amount of damage to be delt
    public int maxHitRegistrations = 1;                 // max amount of enemies that can be hit with attack

    private List<int> hitRegistry = new List<int>();    // list of object id's that were hit
    private int hitsRegistered = 0;                     // amount of objects hit in attack

    private void Start()
    {
        if (targets.Length == 0)
        {
            print(name + " does not have targets set up!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitsRegistered >= maxHitRegistrations)
        {
            //print("max enemies hit");
            return;
        }

        // iterate through targets and compare with 'other.tag'
        foreach (string target in targets)
        {
            int otherID = other.gameObject.GetInstanceID();

            if (other.tag == target && !hitRegistry.Contains(otherID)) // also check if object id is in list (did we hit the object already)
            {
                if (other.GetComponent<Health>() == null)
                {
                    Debug.LogWarning("Object hit: " + other.gameObject + ", Does not have the 'Health' script attached!");
                    return;
                }

                hitsRegistered++;

                hitRegistry.Add(otherID);
                //printHitRegistry();
                print("hit: " + other + ", hits registered: " + hitsRegistered);

                other.GetComponent<Health>().subtract(damageAmount);
            }
        }
    }

    // resets hit registers for next attack
    public void resetAttack()
    {
        //print("Attack reset");
        hitsRegistered = 0;
        hitRegistry.Clear();
    }

    // prints id's of all objects hit
    private void printHitRegistry()
    {
        print("hit registry id's");
        foreach (int id in hitRegistry)
        {
            print(id);
        }
    }
}