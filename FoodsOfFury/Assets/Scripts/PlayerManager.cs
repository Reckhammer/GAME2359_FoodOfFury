using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages the player overall (health, inventory, UI)
//
// TODO: Overall Development, add UI
//----------------------------------------------------------------------------------------

public class PlayerManager : MonoBehaviour
{
    Inventory inventory;
    GameObject currWeapon;
    GameObject currConsumable;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate += HealthUpdated;
    }

    private void OnDisable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate -= HealthUpdated;
    }

    private void Update()
    {
        // switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equipNextItem(ItemType.Weapon);
        }

        // switch consumable
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equipNextItem(ItemType.Consumable);
        }

        // DEBUG: print inventory
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.printInventory();
        }

        // remove weapon
        if (Input.GetKeyDown(KeyCode.K))
        {
            currWeapon = null;
            inventory.remove(ItemType.Weapon);
        }

        // remove consumable
        if (Input.GetKeyDown(KeyCode.L))
        {
            currConsumable = null;
            inventory.remove(ItemType.Consumable);
        }
    }

    // equips item
    public void equipItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                currWeapon?.SetActive(false);       // set old item inactive
                currWeapon = inventory.get(type);  // get item
                currWeapon?.SetActive(true);        // set new item active
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.get(type);
                currConsumable?.SetActive(true);
                break;
            default:
                break;
        }
    }

    // equips next item
    public void equipNextItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                currWeapon?.SetActive(false);       // set old item inactive
                currWeapon = inventory.next(type);  // get next item 
                currWeapon?.SetActive(true);        // set new item active
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.next(type);
                currConsumable?.SetActive(true);  
                break;
            default:
                break;
        }
    }

    // Does health reactions
    private void HealthUpdated(float amount)
    {
        if (amount == 0)
        {
            doDie();
        }
        else
        {
            print("Player was damaged!");
            // hurt animations?
            // update UI (from GameController)
        }
    }

    // hangles death operations
    private void doDie()
    {
        print("Player died");

        // do death animation?
        // send message to GameController
        //Destroy(gameObject);
    }
}
