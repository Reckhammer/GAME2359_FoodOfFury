using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nPickupable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (other.GetComponent<nPlayerInventory>().addItem(GetComponent<nItem>().type))
        {
            Destroy(this.gameObject);
        }
    }
}
