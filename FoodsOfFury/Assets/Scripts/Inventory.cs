using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages a simple inventory of 'Weapon's and 'Consumable's.
//
// TODO: Overall Development, add constructor for inpector 
//       initilization (Item array paramer and set lists)
//----------------------------------------------------------------------------------------

public class Inventory : MonoBehaviour
{
    public int maxWeapons       = 0;    // max amount of weapons
    public int maxConsumables   = 0;    // max amount of consumables

    private InventoryList weapons;      // list for 'Weapon' objects
    private InventoryList consumables;  // list for 'Consumable' objects

    private void Awake()
    {
        weapons     = new InventoryList(maxWeapons);
        consumables = new InventoryList(maxConsumables);
    }

    // return current item reference in list
    public GameObject get(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.get();
            case ItemType.Consumable:
                return consumables.get();
            default:
                return null;
        }
    }

    // returns the next item reference in list
    public GameObject next(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.next();
            case ItemType.Consumable:
                return consumables.next();
            default:
                return null;
        }
    }

    // add item to list
    public bool add(ItemType type, GameObject item)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return addWeapon(type, item);
            case ItemType.Consumable:
                return addConsumable(type, item);
            default:
                return false;
        }
    }

    // add weapon to list
    private bool addWeapon(ItemType type, GameObject item)
    {
        if (maxWeapons != 0)
        {
            GameObject copy = Instantiate(item, transform.position, transform.rotation, transform); // make copy and parent to gameobject
            Destroy(copy.GetComponent<Pickupable>());           // remove 'Pickupable'
            Destroy(copy.GetComponent<Rigidbody>());            // remove rigibody
            copy.GetComponent<Collider>().enabled = false;      // turn off collider (non trigger)
            copy.name = type.ToString();                        // set name
            copy.SetActive(false);                              // make copy inactive

            if (!weapons.add(copy)) // try to add item to list
            {
                removeWeapon(ItemType.Weapon);  // remove current weapon from list
                weapons.add(copy);              // add failed (list at max), swap item
            }

            return true;
        }
        return false; // return false if max at zero
    }

    // add consumalbe to list
    private bool addConsumable(ItemType type, GameObject item)
    {
        if (maxConsumables != 0)
        {
            GameObject copy = Instantiate(item, transform.position, transform.rotation, transform); // make copy and parent to gameobject
            Destroy(copy.GetComponent<Pickupable>());       // remove 'Pickupable'
            Destroy(copy.GetComponent<Rigidbody>());        // remove rigibody
            copy.GetComponent<Collider>().enabled = false;  // turn off collider (non trigger)
            copy.name = type.ToString();                    // set name
            copy.SetActive(false);                          // make copy inactive

            if (!consumables.add(copy)) // try to add item to list
            {
                removeConsumable(ItemType.Consumable);  // remove current consumable from list
                consumables.add(copy);                  // add failed (list at max), swap item
            }

            return true;
        }
        return false; // return false if at max
    }

    // remove item from inventory
    public void remove(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                removeWeapon(type);
                break;
            case ItemType.Consumable:
                removeConsumable(type);
                break;
        }
    }

    // removes current weapon
    private void removeWeapon(ItemType type)
    {
        GameObject reference = weapons.get(); // get reference from list

        if (reference == null)
        {
            return; // there is no object to drop, return
        }

        drop(reference, type);  // drop new copy
        weapons.delete();       // delete reference from list
        Destroy(reference);     // destroy original item gameobject
    }

    // removes current consumable
    private void removeConsumable(ItemType type)
    {
        GameObject reference = consumables.get(); // get reference from list

        if (reference == null)
        {
            return; // there is no object to drop, return
        }

        drop(reference, type);  // drop new copy
        consumables.delete();   // delete reference from list
        Destroy(reference);     // destroy original item gameobject
    }

    // drop item in world
    private void drop(GameObject item, ItemType type)
    {
        GameObject copy = Instantiate(item);
        Pickupable p = copy.AddComponent<Pickupable>();                         // Add 'Pickable' component
        p.type = type;                                                          // set item type
        copy.AddComponent<Rigidbody>();                                         // add rigidbody
        copy.GetComponent<Collider>().enabled = true;                           // turn on collider (non trigger)
        copy.transform.position = transform.position + (transform.forward * 5); // set item in front of this object
        copy.transform.rotation = transform.rotation;                           // set rotation to this rotation
        copy.name = type.ToString();                                            // set name to type
        copy.SetActive(true);                                                   // make item active
    }

    // prints inventory items
    public void printInventory()
    {
        weapons.printList();
        consumables.printList();
    }
}
