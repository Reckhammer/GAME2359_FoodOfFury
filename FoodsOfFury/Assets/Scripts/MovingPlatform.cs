using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public LayerMask stickTo;

    private void Start()
    {
        // if scale is not (1, 1, 1), print warning and set object inactive
        if (transform.localScale != Vector3.one)
        {
            Debug.LogWarning("WARNING: " + gameObject.name + "'s scale needs to be (1, 1, 1) or it will cause scaling issues!!!");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null) // check if parent
        {
            if (stickTo == (stickTo | (1 << other.gameObject.layer))) // check if object is in layermask
            {
                //print("parenting: " + other.gameObject);
                other.transform.SetParent(transform); // set parent to parent of this obj
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform) // check if this is parent
        {
            //print("unparenting: " + other.gameObject);
            other.transform.SetParent(null);
        }
    }
}
