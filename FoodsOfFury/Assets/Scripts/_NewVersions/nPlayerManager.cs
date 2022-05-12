using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//----------------------------------------------------------------------------------------
// Author(s): Jose Villanueva, Abdon J. Puente IV
//
// Description: This class manages the scripts on the player (health, inventory, UI)
//
// TODO: Remove non player scripts references
//----------------------------------------------------------------------------------------

public class nPlayerManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject itemSelection = null;     // to check if player is over a pickable object

    public PostProcessVolume PPV;               // Post Process Volumw reference
    public GameObject hitParticle;              // Gets hit particle
    private int maxLives = 3;                   // Sets max lives
    public int currentLives;
    public bool fullyDied = false;
    private nPlayerInventory inventory;         // inventory reference
    private float oldHealth = 0.0f;             // old amount of health
    private Vignette healthVignette;            // Vignette reference
    private Coroutine vigTimer = null;          // Vignette fade coroutine timer reference
    private Coroutine switchTimer = null;       // switchTimer coroutine reference
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
        PPV.profile.TryGetSettings(out healthVignette);
        healthVignette.intensity.value = 0.0f;
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
            UIManager.instance?.updateHealthBar(amount);
            fullyDied = true;
            doDie();
        }
        else if (amount == 0 && currentLives != 0)
        {
            currentLives--;
            UIManager.instance?.updateHealthBar(amount);
            UIManager.instance.updateLivesUI(currentLives);
        }
        else if (amount < oldHealth) // player damaged
        {
            print("Player was damaged!");
            // hurt animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Hurt_1", "Rollo_Hurt_2", "Rollo_Hurt_3").transform.SetParent(transform);
            Instantiate(hitParticle, transform.position, transform.rotation);
            healthVignette.intensity.value = 0.7f;
            nUIManager.instance?.updateHealthBar(amount);
        }
        else if (amount > oldHealth) // player healed
        {
            print("Player was healed!");
            // healed animations?
            AudioManager.Instance.playRandom(transform.position, "Rollo_Health_01", "Rollo_Health_02").transform.SetParent(transform);
            nUIManager.instance?.updateHealthBar(amount);
        }

        oldHealth = amount;

        if (amount <= 2f && healthVignette.intensity.value != 0.65f)
        {

            if (vigTimer == null)
            {
                vigTimer = StartCoroutine(vigTime(2.0f, 0.65f));
            }
            else
            {
                StopCoroutine(vigTimer);
                vigTimer = StartCoroutine(vigTime(2.0f, 0.65f));
            }
        }

        if (amount > 2f && healthVignette.intensity.value != 0.0f)
        {
            if (vigTimer == null)
            {
                vigTimer = StartCoroutine(vigTime(2.0f, 0.0f));
            }
            else
            {
                StopCoroutine(vigTimer);
                vigTimer = StartCoroutine(vigTime(2.0f, 0.0f));
            }
        }
    }

    private IEnumerator vigTime(float duration, float value)
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

    public void addSwitchDelay(float duration)
    {
        if (switchTimer != null)
        {
            StopCoroutine(switchTimer);
        }

        switchTimer = StartCoroutine(switchingDelay(duration));
    }

    // weapon switching timer
    private IEnumerator switchingDelay(float duration)
    {
        float passed = 0.0f;
        canSwitch = false;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        canSwitch = true;
    }

    // hangles death operations
    private void doDie()
    {
        print("Player died");

        currentLives--;
        UIManager.instance.updateLivesUI(currentLives);

        GetComponent<PlayerMovementTwo>().stopInput(10.0f, true, true);
        // do death animation?
        GetComponent<Animator>().SetTrigger("Death");
        // send message to GameController
    }

    public void sendEvent(string eventName)
    {
        playerEvent(eventName);
    }
}
