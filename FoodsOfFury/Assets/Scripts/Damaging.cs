﻿using System.Collections;
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
    public string[] targets;                        // targets to damage
    public float damageAmount           = 0.0f;     // amount of damage to be delt
    public float delayAmount            = 0.0f;     // time delay to be able damage again
    public bool destroyOnImpact         = false;    // option to destroy on impact
    public bool doesKnockback           = false;    // option for knockback
    public float knockbackForce         = 0.0f;     // force of knockback

    private void OnTriggerEnter(Collider other)
    {
        if (targets.Length == 0)
        {
            print(name + " does not have targets set up!");
            return;
        }

        if (other.transform.parent == null) // check if parent
        {
            // iterate through targets and compare with 'other.tag'
            foreach (string target in targets)
            {
                if (other.tag == target)
                {
                    other.GetComponentInParent<Health>().subtract(damageAmount, delayAmount); // subtract from other's 'health' and add delay

                    if (doesKnockback)
                    {
                        doKnockback(other.gameObject);
                    }
                }
            }
        }

        if (destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }

    // does specific knockback cases
    private void doKnockback(GameObject obj)
    {
        Vector3 dir = (obj.transform.position - transform.position).normalized;

        switch (obj.tag)
        {
            case "Player":
                obj.GetComponentInParent<PlayerMovement>().applyExtraForce(dir * knockbackForce, 0.1f);
                break;
            default:
                obj.GetComponentInParent<Rigidbody>().AddForce(dir * knockbackForce, ForceMode.VelocityChange); // apply basic knockback
                break;
        }
    }
}
