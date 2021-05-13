﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages a simple inventory of 'Weapon's and 'Consumable's.
//----------------------------------------------------------------------------------------

public class Inventory : MonoBehaviour
{
    public GameObject[] startingWeapons;        // weapons to start with
    public GameObject[] startingConsumables;    // consumables to start with
    public int maxWeapons       = 0;            // max amount of weapons
    public int maxConsumables   = 0;            // max amount of consumables
    public int collectibleCount = 0;            //current number of collectibles that the player has
    public int keyCount         = 0;            //current number of keys that the player has

    private InventoryList weapons;              // list for 'Weapon' objects
    private InventoryList consumables;          // list for 'Consumable' objects

    private void Awake()
    {
        weapons     = new InventoryList(maxWeapons);
        consumables = new InventoryList(maxConsumables);

        // set starting weapons
        if (startingWeapons != null)
        {
            foreach (GameObject weapon in startingWeapons)
            {
                if (!weapons.atMax())
                {
                    addToList(ref weapons, weapon, ItemType.Weapon); // add until we reach max
                }
                else
                {
                    print(gameObject.name + ": can't add anymore weapons. Inventory is at max weapons!");
                    break;
                }
            }
        }

        // sets starting consumables
        if (startingConsumables != null)
        {
            foreach (GameObject consumable in startingConsumables)
            {
                if (!consumables.atMax())
                {
                    addToList(ref consumables, consumable, ItemType.Consumable); // add until we reach max
                }
                else
                {
                    print(gameObject.name + ": can't add anymore consumables. Inventory is at max consumables!");
                    break;
                }
            }
        }
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
    public GameObject next(ItemType type, bool updateCurrent = true)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.next(updateCurrent);
            case ItemType.Consumable:
                return consumables.next(updateCurrent);
            default:
                return null;
        }
    }

    // returns the previous item reference in list
    public GameObject previous(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.previous();
            case ItemType.Consumable:
                return consumables.previous();
            default:
                return null;
        }
    }

    // add item to correct list
    public bool add(GameObject item, ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return addToList(ref weapons, item, type);
            case ItemType.Consumable:
                return addToList(ref consumables, item, type);
            default:
                return false;
        }
    }

    // adds item to list
    private bool addToList(ref InventoryList list, GameObject item, ItemType type)
    {
        if (list.max != 0 && list.amount() != list.max)
        {
            GameObject copy = Instantiate(item, transform.position, transform.rotation, transform); // make copy and parent to gameobject
            copy.GetComponent<Pickupable>().render.enabled = false; // turn off renderer

            if (copy.GetComponent<Pickupable>().particles != null) // turn off current particles and set play on awake off
            {
                copy.GetComponent<Pickupable>().particles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                ParticleSystem.MainModule m = copy.GetComponent<Pickupable>().particles.main;
                m.playOnAwake = false;
            }

            if ( copy.GetComponent<Animator>() != null )
            {
                copy.GetComponent<Animator>().enabled = false;
            }

            copy.GetComponent<Pickupable>().enabled = false;        // turn off 'Pickupable'
            //Destroy(copy.GetComponent<Rigidbody>());                // remove rigibody
            copy.GetComponent<Collider>().enabled = false;          // turn off collider (non trigger)
            copy.name = type.ToString();                            // set name
            copy.SetActive(false);                                  // make copy inactive

            list.add(copy);
            //if (!list.add(copy)) // try to add item to list
            //{
            //    removeFromList(ref list, type);                     // add failed (list at max), remove current item from list
            //    list.add(copy);                                     // add item (swap)
            //}

            return true;
        }
        return false; // return false max is 0
    }

    // remove item from inventory
    public void remove(ItemType type, bool dropItemInWorld = true)
    {
        switch (type)
        {
            case ItemType.Weapon:
                removeFromList(ref weapons, type, dropItemInWorld);
                break;
            case ItemType.Consumable:
                removeFromList(ref consumables, type, dropItemInWorld);
                break;
        }
    }

    // removes current consumable
    private void removeFromList(ref InventoryList list, ItemType type, bool dropItemInWorld = true)
    {
        GameObject reference = list.get(); // get reference from list

        if (reference == null)
        {
            return; // there is no object to drop, return
        }

        if (dropItemInWorld)
        {
            if (type == ItemType.Weapon)
            {
                reference.GetComponent<WeaponReferences>().weaponScript.enabled = false; // turn off weapon script
            }

            reference.GetComponent<Pickupable>().render.enabled = true;     // turn on renderer

            if (reference.GetComponent<Pickupable>().particles != null) // turn on particle play on awake (particles will turn on when dropped)
            {
                ParticleSystem.MainModule m = reference.GetComponent<Pickupable>().particles.main;
                m.playOnAwake = true;
            }
            if ( reference.GetComponent<Animator>() != null )
            {
                reference.GetComponent<Animator>().enabled = true;
            }

            drop(reference, type);  // drop new copy
        }

        list.delete();          // delete reference from list
        Destroy(reference);     // destroy original item gameobject

        // give player starting weapon when weapons list is empty
        if (type == ItemType.Weapon && list.isEmpty() && startingWeapons.Length != 0)
        {
            addToList(ref weapons, startingWeapons[0], ItemType.Weapon); // add starting weapon as fallback
            GetComponent<PlayerManager>().equipItem(ItemType.Weapon);    // call player manager to equip weapon
        }
    }

    // drop item in world
    private void drop(GameObject item, ItemType type)
    {
        GameObject copy = Instantiate(item);
        copy.GetComponent<Pickupable>().enabled = true;                         // turn on 'Pickupable'
        //Rigidbody rb = copy.AddComponent<Rigidbody>();                          // add rigidbody
        copy.GetComponent<Collider>().enabled = true;                           // turn on collider (non trigger)
        copy.transform.position = transform.position;                           // set item position to this object
        copy.transform.rotation = transform.rotation;                           // set rotation to this rotation
        copy.name = type.ToString();                                            // set name to type
        copy.GetComponent<Pickupable>().doTimeout();                            // set timeout for pickable

        copy.SetActive(true);                                                   // make item active
        //rb.AddForce(transform.forward * 5.0f, ForceMode.VelocityChange);        // throw item
    }

    // returns amount of items in type of list
    public int amount(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.amount();
            case ItemType.Consumable:
                return consumables.amount();
            default:
                return 0;
        }
    }

    // checks list if there is an gameobject that has component type and returns it
    public GameObject findFromScript<T>(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return weapons.findFromScript<T>();
            case ItemType.Consumable:
                return consumables.findFromScript<T>();
            default:
                return null;
        }
    }

    // prints inventory items
    public void printInventory()
    {
        weapons.printList();
        consumables.printList();
    }

    //adds key to player's inventory and updates UI
    public void addKey( int amt = 1 )
    {
        keyCount += amt;
        UIManager.instance.updateKeyUI();
    }
}
