using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnable : MonoBehaviour
{
    public GameObject objectEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            objectEvent.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
