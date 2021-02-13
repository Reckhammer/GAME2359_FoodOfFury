using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Allows the ability to add an item to inventory.
//
// TODO: add check for button push
//----------------------------------------------------------------------------------------

public class Pickupable : MonoBehaviour
{
    private Item item; // item that will be moved into an inventory

    void Start()
    {
        item = (Item)gameObject.GetComponent(typeof(Item));
    }

    // needs to be changed to a button push
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponentInParent<Inventory>().add(item))
            {
                Destroy(gameObject); // item succesfully added, delete this object
            }
        }
    }
}
