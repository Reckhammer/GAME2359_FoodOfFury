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
    private Inventory inventory;        // inventory reference
    private GameObject currWeapon;      // current reference to weapon object
    private GameObject currConsumable;  // current reference to consumable object
    private float oldHealth = 0.0f;     // old amount of health

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        equipItem(ItemType.Weapon);
        equipItem(ItemType.Consumable);
        oldHealth = GetComponent<Health>().amount;
    }

    // subscribe to Health.OnUpdate() event
    private void OnEnable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate += HealthUpdated;
    }

    // unsubscribe to Health.OnUpdate() event
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

        // use consumable
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currConsumable != null) // if currConsumable exists
            {
                if (currConsumable.GetComponent<Consumable>().use(gameObject)) // if health was added
                {
                    currConsumable = null;                          // set currConsumable to null
                    inventory.remove(ItemType.Consumable, false);   // remove consumable from inventory (dont drop in world)
                    equipNextItem(ItemType.Consumable);             // equip next consumable
                }
            }
        }
    }

    // equips item
    public void equipItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                currWeapon?.SetActive(false);       // set old item inactive
                currWeapon = inventory.get(type);   // get item
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
        if (amount == 0) // // player died
        {
            doDie();
        }
        else if (amount < oldHealth) // player damaged
        {
            print("Player was damaged!");
            // hurt animations?
            // update UI (from GameController)
        }
        else if (amount > oldHealth) // player healed
        {
            print("Player was healed!");
            // healed animations?
            // update UI (from GameController)
        }

        oldHealth = amount;
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
