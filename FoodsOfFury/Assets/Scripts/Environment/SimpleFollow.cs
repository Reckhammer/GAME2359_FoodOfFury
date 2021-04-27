using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public GameObject target;

    void FixedUpdate()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }
}
