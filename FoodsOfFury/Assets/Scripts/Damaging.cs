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

        // iterate through targets and compare with 'other.tag'
        foreach (string target in targets)
        {
            if (other.tag == target)
            {
                //print("collided!!!");
                other.GetComponentInParent<Health>().subtract(damageAmount, delayAmount); // subtract from other's 'health' and add delay

                if (doesKnockback)
                {
                    Vector3 dir = (other.transform.position - transform.position).normalized;
                    other.GetComponentInParent<Rigidbody>().AddForce(dir * knockbackForce, ForceMode.VelocityChange); // apply basic knockback
                }
            }
        }

        if (destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }
}
