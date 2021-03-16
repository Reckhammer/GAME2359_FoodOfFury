using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages the player overall (health, inventory, UI)
//
// TODO: Overall Development
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

        if (GameController.instance != null)
        {
            GameController.instance.setHealthBarMax(GetComponent<Health>().max);
            GameController.instance.updateHealthBar(oldHealth);
        }
        else
        {
            print("GameController missing!!!");
        }
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
            equipNextItem(ItemType.Weapon);
        }

        // remove consumable
        if (Input.GetKeyDown(KeyCode.L))
        {
            currConsumable = null;
            inventory.remove(ItemType.Consumable);
            equipNextItem(ItemType.Consumable);
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

                if (currWeapon != null) // update UI
                {
                    GameController.instance.updateWeaponUI(currWeapon.GetComponent<Weapon>().sprite);
                }
                else
                {
                    GameController.instance.updateWeaponUI(null);
                }
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.get(type);
                currConsumable?.SetActive(true);

                if (currConsumable != null) // update UI
                {
                    GameController.instance.updateConsumablesUI(currConsumable.GetComponent<Consumable>().sprite);
                }
                else
                {
                    GameController.instance.updateConsumablesUI(null);
                }
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

                if (currWeapon != null) // update UI
                {
                    GameController.instance.updateWeaponUI(currWeapon.GetComponent<Weapon>().sprite);
                }
                else
                {
                    GameController.instance.updateWeaponUI(null);
                }
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.next(type);
                currConsumable?.SetActive(true);

                if (currConsumable != null) // update UI
                {
                    GameController.instance.updateConsumablesUI(currConsumable.GetComponent<Consumable>().sprite);
                }
                else
                {
                    GameController.instance.updateConsumablesUI(null);
                }
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
            GameController.instance?.updateHealthBar(amount);
            doDie();
        }
        else if (amount < oldHealth) // player damaged
        {
            print("Player was damaged!");
            AudioManager.Instance.playRandom(transform.position, "Rollo_Hurt_1", "Rollo_Hurt_2", "Rollo_Hurt_3").transform.SetParent(transform);
            // hurt animations?
            GameController.instance?.updateHealthBar(amount);
        }
        else if (amount > oldHealth) // player healed
        {
            print("Player was healed!");
            // healed animations?
            GameController.instance?.updateHealthBar(amount);
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
