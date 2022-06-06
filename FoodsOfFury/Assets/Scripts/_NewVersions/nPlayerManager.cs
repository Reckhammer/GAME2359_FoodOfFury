using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author(s): Jose Villanueva, Abdon J. Puente IV
//
// Description: This class manages the scripts on the player (health, inventory, UI)
//
// TODO: Remove non player scripts references
//----------------------------------------------------------------------------------------

public class nPlayerManager : MonoBehaviour
{
    public GameObject hitParticle;              // Gets hit particle
    private int maxLives = 3;                   // Sets max lives
    public int currentLives;
    public bool fullyDied = false;
    private nPlayerInventory inventory;         // inventory reference
    private float oldHealth = 0.0f;             // old amount of health
    private bool canSwitch = true;              // if able to switch weapons

    public delegate void PlayerAnimationEvent(string message);
    public event PlayerAnimationEvent playerEvent;

    private void Start()
    {
        oldHealth = GetComponent<nHealth>().max;
        currentLives = maxLives;
        nUIManager.instance.updateLivesUI(currentLives);
        nUIManager.instance.setHealthBarMax(oldHealth);
        nUIManager.instance.updateHealthBar(oldHealth);
        inventory = GetComponent<nPlayerInventory>();
    }

    // subscribe to Health.OnUpdate() event
    private void OnEnable()
    {
        nHealth health = GetComponent<nHealth>();
        health.OnUpdate += HealthUpdated;
    }

    // unsubscribe to Health.OnUpdate() event
    private void OnDisable()
    {
        nHealth health = GetComponent<nHealth>();
        health.OnUpdate -= HealthUpdated;
    }

    private void Update()
    {
        // equip next weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && canSwitch && !PauseMenu.gameIsPaused)
        {
            inventory.equipNextWeapon();
        }

        // equip previous weapon
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && canSwitch && !PauseMenu.gameIsPaused)
        {
            inventory.equipPreviousWeapon();
        }

        // use consumable
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.useHealthPickup();
        }
    }

    // Does health reactions
    private void HealthUpdated(float amount)
    {
        if (amount == 0 && currentLives == 1) // // player died
        {
            nUIManager.instance?.updateHealthBar(amount);
            fullyDied = true;
            doDie();
        }
        else if (amount == 0 && currentLives != 0)
        {
            currentLives--;
            nUIManager.instance?.updateHealthBar(amount);
            nUIManager.instance?.updateLivesUI(currentLives);
        }
        else if (amount < oldHealth) // player damaged
        {
            //print("Player was damaged!");
            // hurt animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Hurt_1", "Rollo_Hurt_2", "Rollo_Hurt_3").transform.SetParent(transform);
            Instantiate(hitParticle, transform.position, transform.rotation);
            nUIManager.instance?.updateHealthBar(amount);

            nUIManager.instance?.setVignetteIntensity(0.65f);
            if (amount > 2.0f)
            {
                nUIManager.instance?.doVignetteFadeEffect(2.0f, 0.0f);
            }
        }
        else if (amount > oldHealth) // player healed
        {
            //print("Player was healed!");
            // healed animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Health_01", "Rollo_Health_02").transform.SetParent(transform);
            nUIManager.instance?.updateHealthBar(amount);

            if (amount > 2.0f)
            {
                nUIManager.instance?.doVignetteFadeEffect(2.0f, 0.0f);
            }
        }

        oldHealth = amount;
    }

    public void enableWeaponSwitch(bool enable)
    {
        canSwitch = enable;
    }

    // hangles death operations
    private void doDie()
    {
        print("Player died");

        currentLives--;
        nUIManager.instance.updateLivesUI(currentLives);
        GetComponent<nPlayerMovement>().stopInput(true);
        GetComponent<nPlayerMovement>().stopRotation(true);
        GetComponent<Animator>().SetTrigger("Death");
    }

    public void sendEvent(string eventName)
    {
        if (playerEvent != null)
        {
            playerEvent(eventName);
        }
    }
}
