using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class acts as a UI manager.
//----------------------------------------------------------------------------------------

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; } // GameController instance

    public HealthBar healthBar;             // reference to player health bar
    public Image weaponImageUI;             // reference to weapon image
    public Image[] weaponSelectedOverlays;  // references to weapon overlays
    public Image oldWeaponImageUI;          // reference to previous image
    public Image consumableImageUI;         // reference to consumable image
    public Text consumableAmountUI;         // reference to consumable amount text
    public Text collectibleAmoutUI;         // reference to collectible amount text
    public Text keyAmountUI;                // reference to key amount text
    public Text objectivesText;             // referenct to objectives text

    // do singleton stuff
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        if (healthBar == null)
        {
            print("healthBar is not set up on GameController");
        }
    }

    // update health bar
    public void updateHealthBar(float amount)
    {
        healthBar?.updateHealthBar(amount);
    }

    // set heath bar max
    public void setHealthBarMax(float max)
    {
        healthBar?.setHealthBarMax(max);
    }

    // updates the weapoons UI
    public void updateWeaponUI(Sprite current, Sprite old)
    {
        if (weaponImageUI == null)
        {
            return;
        }

        weaponImageUI.sprite = current;
        oldWeaponImageUI.sprite = old;

        if (weaponImageUI.sprite != null)
        {
            foreach (Image image in weaponSelectedOverlays)
            {
                image.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Image image in weaponSelectedOverlays)
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    // updates the consumables UI
    public void updateConsumablesUI(Sprite image, float amount)
    {
        if (consumableImageUI == null)
        {
            return;
        }

        consumableImageUI.sprite = image;
        consumableAmountUI.text = "x" + amount;
    }

    //update the collectible UI
    public void updateCollectibleUI()
    {
        // find with tag is evil
        Inventory playerInventory = GameObject.FindWithTag( "Player" ).GetComponentInParent<Inventory>();

        collectibleAmoutUI.text = ""+ playerInventory.collectibleCount;
    }

    //update the key UI
    public void updateKeyUI()
    {
        // find with tag is evil
        Inventory playerInventory = GameObject.FindWithTag( "Player" ).GetComponentInParent<Inventory>();

        keyAmountUI.text = ""+ playerInventory.keyCount;
    }

    public void setObjectiveText(string text)
    {
        objectivesText.text = text;
    }
}
