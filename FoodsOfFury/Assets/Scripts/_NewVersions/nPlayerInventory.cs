using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Manages an inventory of weapons, consumable and items. It is currently 
//              designed around a revolving style of weapons selection
//----------------------------------------------------------------------------------------

public class nPlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInfo
    {
        public GameObject weaponRef;
        public bool inInventory;
    }

    public WeaponInfo[] weapons;                    // ref to weapons to be enabled/disabled
    public int maxHealthPickups = 0;                // max amount of healthpickups able to be added
    public int healthPickups { get; private set; }  // amount of health pickups currently
    public int keyCount { get; private set; }       // amount of keys in inventory
    public int starFruit { get; private set; }      // amount of star fruit in inventory

    private int currentWeapon   = 0;                // index of current weapon in inventory

    private void Start()
    {
        // set first weapon active
        if (weapons != null)
        {
            weapons[0].weaponRef.SetActive(true);
            nUIManager.instance.setWeaponOverlayActive(0);
        }

        // set weapons icons
        for (int x = 0; x < weapons.Length; x++)
        {
            if (weapons[x].inInventory)
            {
                nUIManager.instance.setWeaponUI(x, weapons[x].weaponRef.GetComponent<nItem>().type);
            }
        }
    }

    // equip next weapon in inventory
    public void equipNextWeapon()
    {
        int oldWeaponIndex = currentWeapon;     // old weapon index to compare
        int count = weapons.Length;             // length to cycle through

        // cycle through weapons until next inInventory is reached
        while (count != 0)
        {
            currentWeapon = (currentWeapon == weapons.Length - 1) ? 0 : currentWeapon + 1;
            if (weapons[currentWeapon].inInventory)
            {
                break;
            }
        }

        // change only when new weapon is equiped
        if (currentWeapon != oldWeaponIndex)
        {
            weapons[oldWeaponIndex].weaponRef.SetActive(false); // set old weapon inactive
            weapons[currentWeapon].weaponRef.SetActive(true);   // set new weapon active
        }

        nUIManager.instance.setWeaponOverlayInactive(oldWeaponIndex);
        nUIManager.instance.setWeaponOverlayActive(currentWeapon);
    }

    // equip previous weapon in inventory
    public void equipPreviousWeapon()
    {
        int oldWeaponIndex = currentWeapon;     // old weapon index to compare
        int count = weapons.Length;             // length to cycle through

        // cycle through weapons until next inInventory is reached
        while (count != 0)
        {
            currentWeapon = (currentWeapon == 0) ? weapons.Length - 1 : currentWeapon - 1;
            if (weapons[currentWeapon].inInventory)
            {
                break;
            }
        }

        // change only when new weapon is equiped
        if (currentWeapon != oldWeaponIndex)
        {
            weapons[oldWeaponIndex].weaponRef.SetActive(false); // set old weapon inactive
            weapons[currentWeapon].weaponRef.SetActive(true);   // set new weapon active
        }

        nUIManager.instance.setWeaponOverlayInactive(oldWeaponIndex);
        nUIManager.instance.setWeaponOverlayActive(currentWeapon);
    }

    // adds item to inventory
    public bool addItem(nItemType item)
    {
        switch (item)
        {
            case nItemType.Key:
                addKey();
                return true;
            case nItemType.StarFruit:
                addStarFruit();
                return true;
            case nItemType.HealthPickup:
                return addHealthPickup();
            case nItemType.KetchupWeapon:
                return addWeaponInInventory(item);
            default:
                return false;
        }
    }

    // add a health pickup
    public bool addHealthPickup()
    {
        if (healthPickups != maxHealthPickups)
        {
            healthPickups++;
            nUIManager.instance.setConsumablesUI(nItemType.HealthPickup, healthPickups);
            return true;
        }
        else
        {
            return false;
        }
    }

    // add a key
    public void addKey()
    {
        keyCount++;
        nUIManager.instance.setKeyUI(keyCount);
    }

    // add a star fruit
    public void addStarFruit()
    {
        starFruit++;
        nUIManager.instance.setStarFruitUI(starFruit);
    }

    // enable inInventory for selected weapon script
    private bool addWeaponInInventory(nItemType type)
    {
        int index = 0;
        bool success = false;

        switch (type) // might become usefull later when adding more weapons
        {
            case nItemType.KetchupWeapon:
                index = 1;
                success = weapons[1].weaponRef.GetComponent<nKetchupWeapon>().reload();
                break;
            default:
                return false;
        }

        if (success)
        {
            weapons[index].inInventory = true;
            nUIManager.instance.setWeaponUI(index, type);
            return true;
        }
        return false;
    }

    // disables current weapon from being used
    public void disableCurrentWeapon()
    {
        weapons[currentWeapon].inInventory = false;
        nUIManager.instance.setWeaponUI(currentWeapon, nItemType.None);
        equipNextWeapon();
    }

    // use health pickup
    public bool useHealthPickup()
    {
        if (healthPickups > 0 && GetComponent<nHealth>().add(2))
        {
            healthPickups--;

            if (healthPickups == 0)
            {
                nUIManager.instance.setConsumablesUI(nItemType.None, 0.0f);
            }
            else
            {
                nUIManager.instance.setConsumablesUI(nItemType.HealthPickup, healthPickups);
            }
            return true;
        }

        return false;
    }
}