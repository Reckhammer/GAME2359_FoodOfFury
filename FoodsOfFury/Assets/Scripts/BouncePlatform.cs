using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Acts as a bouncing platform throwing selected gameobjects in a direction
//              with a force.
//----------------------------------------------------------------------------------------

public class BouncePlatform : MonoBehaviour
{
    public LayerMask allowBounce;                                   // objects that can bounce from platform
    public float bounceForce                        = 1.0f;         // force of bounce
    public Vector3 direction                        = Vector3.up;   // direction of bounce
    public bool useRotationAsDirection              = false;        // to use objects Vector3.up as the direction
    public Transform calculateDirectionToPosition   = null;         // to use a position to calculate direction

    private void Start()
    {
        if (useRotationAsDirection) // use transform.up as direction
        {
            direction = transform.up;
        }
        else if (calculateDirectionToPosition != null) // calculate direction from this position to calculateDirectionToPosition
        {
            direction = (calculateDirectionToPosition.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) // check if parent
        {
            if (allowBounce == (allowBounce | (1 << other.gameObject.layer))) // check if object is in layermask
            {
                doBounce(other.gameObject);
            }
        }
    }

    // does specific bounce cases
    private void doBounce(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Player":
                obj.GetComponentInParent<PlayerMovement>().applyExtraForce(direction * bounceForce);
                break;
            default:
                obj.GetComponent<Rigidbody>().AddForce(direction * bounceForce, ForceMode.VelocityChange); // apply basic bounce force
                break;
        }
    }
}
