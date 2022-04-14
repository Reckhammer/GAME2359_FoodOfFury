using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages a simple inventory of 'Weapon's and 'Consumable's.
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

    private int currentWeapon   = 0;                // index of current weapon in inventory

    private void Start()
    {
        int weaponsSetup = 0;

        //setup inital weapons
        foreach (WeaponInfo weapon in weapons)
        {
            if (weapon.weaponRef == null)
            {
                Debug.LogWarning("nPlayerInventory: weapon in element " + currentWeapon + " is empty!");
            }
            else if (weaponsSetup < 2 && weapon.inInventory)
            {
                if (weaponsSetup == 0)
                {
                    weapon.weaponRef.SetActive(true);
                    nUIManager.instance.setWeaponOneUI(weapon.weaponRef.GetComponent<WeaponReferences>().sprite);
                }
                else
                {
                    nUIManager.instance.setWeaponTwoUI(weapon.weaponRef.GetComponent<WeaponReferences>().sprite);
                }
                weaponsSetup++;
            }

            if (weaponsSetup == 0)
            {
                currentWeapon++;
            }
        }
    }

    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            addHealthPickup();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            addKey();
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
            nUIManager.instance.setWeaponOneUI(weapons[currentWeapon].weaponRef.GetComponent<WeaponReferences>().sprite);

            if (weapons[oldWeaponIndex].inInventory) // update UI if old weapon is still inInventory
            {
                nUIManager.instance.setWeaponTwoUI(weapons[oldWeaponIndex].weaponRef.GetComponent<WeaponReferences>().sprite);
            }
            else
            {
                nUIManager.instance.setWeaponTwoUI(null);
            }
        }
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
            nUIManager.instance.setWeaponOneUI(weapons[currentWeapon].weaponRef.GetComponent<WeaponReferences>().sprite);

            if (weapons[oldWeaponIndex].inInventory) // update UI if old weapon is still inInventory
            {
                nUIManager.instance.setWeaponTwoUI(weapons[oldWeaponIndex].weaponRef.GetComponent<WeaponReferences>().sprite);
            }
            else
            {
                nUIManager.instance.setWeaponTwoUI(null);
            }
        }
    }

    // add a health pickup
    public void addHealthPickup()
    {
        if (healthPickups != maxHealthPickups)
        {
            healthPickups++;
        }
    }

    // add a key
    public void addKey()
    {
        keyCount++;
        nUIManager.instance.setKeyUI(keyCount);
    }

    // disables current weapon from being used
    public void disableCurrentWeapon()
    {
        weapons[currentWeapon].inInventory = false;
        equipNextWeapon();
    }
}