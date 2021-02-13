using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Allows the ability to add an item to inventory.
//----------------------------------------------------------------------------------------

public class Pickupable : MonoBehaviour
{
    private Item item;                  // item that will be moved into an inventory
    private GameObject player = null;   // target that can pick item up
    private bool canPickUp = false;     // to check if pickup is ready

    void Start()
    {
        item = (Item)gameObject.GetComponent(typeof(Item));
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            if (player.GetComponentInParent<Inventory>().add(item))
            {
                Destroy(gameObject); // item succesfully added, delete this object
            }
        }
    }

    // player is inside trigger, check for input
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = true;
            player = other.gameObject;
        }
    }

    // player left trigger, stop checking for input
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = false;
            player = null;
        }
    }
}
