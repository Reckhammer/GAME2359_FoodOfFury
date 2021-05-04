using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioSource CollectibleSounds; // -Brian

    void OnTriggerEnter( Collider other )
    {
        //Check if the player has collided with the collectible
        if ( other.tag == "Player" )
        {
            Inventory inv = other.GetComponent<Inventory>();

            if (inv != null) // check if hit player object with the inventory script (player has multiple colliders)
            {
                inv.collectibleCount++;
                UIManager.instance.updateCollectibleUI();
                AudioManager.Instance.playRandom(transform.position, "Pickup_Starfruit_01"); // -Brian
                Destroy(gameObject);
            }
        }
    }
}
