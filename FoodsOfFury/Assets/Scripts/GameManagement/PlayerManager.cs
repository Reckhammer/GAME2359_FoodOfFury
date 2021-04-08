using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages the player overall (health, inventory, UI)
//
// TODO: Overall Development
//----------------------------------------------------------------------------------------

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject itemSelection = null;    // to check if player is over a pickable object

    private Inventory inventory;                // inventory reference
    private GameObject currWeapon;              // current reference to weapon object
    private GameObject currConsumable;          // current reference to consumable object
    private float oldHealth = 0.0f;             // old amount of health

    public PostProcessVolume PPV;
    private Vignette healthVignette;
    private Coroutine cTimer = null;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        equipItem(ItemType.Weapon);
        equipItem(ItemType.Consumable);
        oldHealth = GetComponent<Health>().amount;

        if (UIManager.instance != null)
        {
            UIManager.instance.setHealthBarMax(GetComponent<Health>().max);
            UIManager.instance.updateHealthBar(oldHealth);
        }
        else
        {
            print("GameController missing!!!");
        }

        PPV.profile.TryGetSettings(out healthVignette);
        healthVignette.intensity.value = 0.0f;
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
        // equip next weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !GetComponent<PlayerMovementTwo>().isInputStopped())
        {
            equipNextItem(ItemType.Weapon);
        }

        // equip previous weapon
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !GetComponent<PlayerMovementTwo>().isInputStopped())
        {
            equipPreviousItem(ItemType.Weapon);
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currConsumable != null && itemSelection == null) // if currConsumable exists
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
                Sprite oldImage = null;

                if (currWeapon != null)
                {
                    oldImage = currWeapon.GetComponent<WeaponReferences>().sprite;
                    currWeapon.SetActive(false);       // set old item inactive
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = false; // turn off weapon script
                }

                GameObject old = currWeapon;
                currWeapon = inventory.get(type); // get item

                if (old == currWeapon)
                {
                    oldImage = null;
                }

                if (currWeapon != null) // update UI & turn on weapon script
                {
                    currWeapon.SetActive(true);        // set new item active
                    UIManager.instance.updateWeaponUI(currWeapon.GetComponent<WeaponReferences>().sprite, oldImage);
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = true;
                }
                else
                {
                    UIManager.instance.updateWeaponUI(null, null);
                }
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.get(type);
                currConsumable?.SetActive(true);

                if (currConsumable != null) // update UI
                {
                    UIManager.instance.updateConsumablesUI(currConsumable.GetComponent<Consumable>().sprite, inventory.amount(ItemType.Consumable));
                }
                else
                {
                    UIManager.instance.updateConsumablesUI(null, 0);
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
                Sprite oldImage = null;
                currWeapon = inventory.get(type); // get current (in case it was swapped)

                if (currWeapon != null)
                {
                    oldImage = currWeapon.GetComponent<WeaponReferences>().sprite;
                    currWeapon.SetActive(false);       // set old item inactive
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = false; // turn off weapon script
                }

                GameObject old = currWeapon;
                currWeapon = inventory.next(type);   // get next item

                if (old == currWeapon)
                {
                    oldImage = null;
                }

                if (currWeapon != null) // update UI & turn on weapon script
                {
                    currWeapon.SetActive(true);        // set new item active
                    UIManager.instance.updateWeaponUI(currWeapon.GetComponent<WeaponReferences>().sprite, oldImage);
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = true;
                }
                else
                {
                    UIManager.instance.updateWeaponUI(null, null);
                }
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.next(type);
                currConsumable?.SetActive(true);

                if (currConsumable != null) // update UI
                {
                    UIManager.instance.updateConsumablesUI(currConsumable.GetComponent<Consumable>().sprite, inventory.amount(ItemType.Consumable));
                }
                else
                {
                    UIManager.instance.updateConsumablesUI(null, 0);
                }
                break;
            default:
                break;
        }
    }

    public void equipPreviousItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                Sprite oldImage = null;
                currWeapon = inventory.get(type); // get current (in case it was swapped)

                if (currWeapon != null)
                {
                    oldImage = currWeapon.GetComponent<WeaponReferences>().sprite;
                    currWeapon.SetActive(false);       // set old item inactive
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = false; // turn off weapon script
                }

                GameObject old = currWeapon;
                currWeapon = inventory.previous(type);   // get previous item

                if (old == currWeapon)
                {
                    oldImage = null;
                }

                if (currWeapon != null) // update UI & turn on weapon script
                {
                    currWeapon.SetActive(true);        // set new item active
                    UIManager.instance.updateWeaponUI(currWeapon.GetComponent<WeaponReferences>().sprite, oldImage);
                    currWeapon.GetComponent<WeaponReferences>().weaponScript.enabled = true;
                }
                else
                {
                    UIManager.instance.updateWeaponUI(null, null);
                }
                break;
            case ItemType.Consumable:
                currConsumable?.SetActive(false);
                currConsumable = inventory.previous(type);
                currConsumable?.SetActive(true);

                if (currConsumable != null) // update UI
                {
                    UIManager.instance.updateConsumablesUI(currConsumable.GetComponent<Consumable>().sprite, inventory.amount(ItemType.Consumable));
                }
                else
                {
                    UIManager.instance.updateConsumablesUI(null, 0);
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
            UIManager.instance?.updateHealthBar(amount);
            doDie();
        }
        else if (amount < oldHealth) // player damaged
        {
            print("Player was damaged!");
            // hurt animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Hurt_1", "Rollo_Hurt_2", "Rollo_Hurt_3").transform.SetParent(transform);
            UIManager.instance?.updateHealthBar(amount);
        }
        else if (amount > oldHealth) // player healed
        {
            print("Player was healed!");
            // healed animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Health_01", "Rollo_Health_02").transform.SetParent(transform);
            UIManager.instance?.updateHealthBar(amount);
        }

        oldHealth = amount;

        if (amount <= 2f && healthVignette.intensity.value != 0.65f)
        {

            if (cTimer == null)
            {
                cTimer = StartCoroutine(timer(2.0f, 0.65f));
            }
            else
            {
                StopCoroutine(cTimer);
                cTimer = StartCoroutine(timer(2.0f, 0.65f));
            }
        }

        if (amount > 2f && healthVignette.intensity.value != 0.0f)
        {
            if (cTimer == null)
            {
                cTimer = StartCoroutine(timer(2.0f, 0.0f));
            }
            else
            {
                StopCoroutine(cTimer);
                cTimer = StartCoroutine(timer(2.0f, 0.0f));
            }
        }
    }

    private IEnumerator timer(float duration, float value)
    {
        float passed = 0.0f;

        float original = healthVignette.intensity.value;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            healthVignette.intensity.value = Mathf.Lerp(original, value, passed / duration);
            yield return null;
        }
    }

    // hangles death operations
    private void doDie()
    {
        print("Player died");

        GetComponent<PlayerMovementTwo>().stopInput(10.0f, true, true);
        GetComponent<Animator>().SetTrigger("Death");
        // do death animation?
        // send message to GameController
        //Destroy(gameObject);
    }
}
