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
    public ItemType     type;                       // type of item
    public Renderer     render              = null; // object renderer
    public ParticleSystem particles         = null; // particale system reference

    private GameObject  player      = null;         // target that can pick item up
    private bool        canPickUp   = false;        // to check if pickup is ready
    private bool        inTimeout   = false;        // for when item was just dropped

    private void Update()
    {
        if (canPickUp)
        {
            if (type == ItemType.Consumable)
            {
                if (player.GetComponentInParent<Health>().add(GetComponent<Consumable>().healthAmount))
                {
                    Destroy(gameObject);
                    return;
                }
            }
            else if (type == ItemType.Weapon)
            {
                if (GetComponent<KetchupWeapon>() != null) // check if this object has ketchup weapon script
                {
                    // get gameobject reference from player inventory if ketchup gun exists
                    GameObject temp = player.GetComponentInParent<Inventory>().findFromScript<KetchupWeapon>(ItemType.Weapon);

                    if (temp != null) // check if returned a reference
                    {
                        KetchupWeapon kWeapon = temp.GetComponent<KetchupWeapon>(); // get reference to weapon script
                        //print("bullet amount: " + temp.GetComponent<KetchupWeapon>().bulletAmount);

                        if (kWeapon.bulletAmount != kWeapon.maxBullets) // if weapon is not at max bullets, set to max and destroy this gameobject
                        {
                            kWeapon.bulletAmount = kWeapon.maxBullets;

                            if (kWeapon.enabled) // if weapon is enabled, update UI
                            {
                                UIManager.instance.setWeaponUseUI(kWeapon.maxBullets);
                            }

                            Destroy(gameObject);
                            return;
                        }
                    }
                }
            }

            if (player.GetComponentInParent<Inventory>().add(gameObject, type))
            {
                if (type == ItemType.Consumable)
                {
                    AudioManager.Instance.playRandom(transform.position, "Pickup_Health_02");
                }
                player.GetComponentInParent<PlayerManager>().equipNextItem(type);  // call player manager to equip next item
                player.GetComponentInParent<PlayerManager>().itemSelection = null; // set player item selection to null
                Destroy(gameObject); // item succesfully added, delete this object
            }
        }
    }

    // player is inside trigger, check for input
    private void OnTriggerEnter(Collider other)
    {
        if (inTimeout)
        {
            return;
        }

        if (other.tag == "Player")
        {
            player = other.gameObject;

            // player has item selected, return
            if (player.GetComponentInParent<PlayerManager>().itemSelection != null)
            {
                return;
            }

            canPickUp = true;
            player.GetComponentInParent<PlayerManager>().itemSelection = gameObject;
        }
    }

    // check for pickup while in collider
    private void OnTriggerStay(Collider other)
    {
        if (inTimeout)
        {
            return;
        }

        if (other.tag == "Player")
        {
            player = other.gameObject;

            // player has item selected, return
            if (player.GetComponentInParent<PlayerManager>().itemSelection != null)
            {
                return;
            }

            canPickUp = true;
            player.GetComponentInParent<PlayerManager>().itemSelection = gameObject;
        }
    }

    // player left trigger, stop checking for input
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = false;
            //highlightCr = null;
            if (player != null && player.GetComponentInParent<PlayerManager>().itemSelection == gameObject)
            {
                player.GetComponentInParent<PlayerManager>().itemSelection = null;
            }
            player = null;
        }
    }

    public void doTimeout(float duration = 1.0f)
    {
        StartCoroutine(Timeout(duration));
    }

    private IEnumerator Timeout(float duration)
    {
        float passed = 0.0f;
        inTimeout = true;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        inTimeout = false;
    }
}
