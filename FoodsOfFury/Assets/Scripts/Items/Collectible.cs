using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter( Collider other )
    {
        //Check if the player has collided with the collectible
        if ( other.tag == "Player" )
        {
            GameObject player = other.gameObject;

            player.GetComponent<Inventory>().collectibleCount++;
            UIManager.instance.updateCollectibleUI();
            Destroy( this.gameObject );
        }
    }
}
