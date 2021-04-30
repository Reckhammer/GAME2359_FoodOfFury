using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter( Collider other )
    {
        //Check if the player has collided with the collectible
        if ( other.tag == "Player" )
        {
            //Get a reference to the player's inventory
            Inventory inv = other.GetComponent<Inventory>();

            if ( inv != null ) // check if hit player object with the inventory script (player has multiple colliders)
            {
                inv.addKey();
                UIManager.instance.updateKeyUI();
                Destroy( gameObject );
            }
        }
    }
}
