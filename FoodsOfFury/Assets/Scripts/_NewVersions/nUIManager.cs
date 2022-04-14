using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class acts as a UI manager.
//----------------------------------------------------------------------------------------

public class nUIManager : MonoBehaviour
{
    public static nUIManager instance { get; private set; } // GameController instance

    public HealthBar healthBar;             // reference to player health bar
    public Image weaponOneImageUI;          // reference to weapon one image
    public Image weaponTwoImageUI;          // reference to weapon two image
    public Image[] weaponOneOverlays;       // references to weapon one overlays
    public Image consumableImageUI;         // reference to consumable image
    public Text consumableAmountUI;         // reference to consumable amount text
    public Text collectibleAmoutUI;         // reference to collectible amount text
    public Text keyAmountUI;                // reference to key amount text
    public Text objectivesText;             // reference to objectives text
    public Text weaponUseAmountUI;          // reference to weapon use amount (ex. ketchup shots left)
    public Text livesAmountUI;
    public Slider loadingSlider;            // reference to loading slider for loading screen

    // do singleton stuff
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
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

    //// updates the weapoons UI
    //public void updateWeaponUI(Sprite current, Sprite old)
    //{
    //    //if (weaponImageUI == null)
    //    //{
    //    //    return;
    //    //}

    //    if (current == null)
    //    {
    //        weaponImageUI.gameObject.SetActive(false);
    //        oldWeaponImageUI.gameObject.SetActive(false);
    //        return;
    //    }

    //    if (old == null)
    //    {
    //        oldWeaponImageUI.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        oldWeaponImageUI.gameObject.SetActive(true);
    //    }
    //    weaponImageUI.gameObject.SetActive(true);

    //    weaponImageUI.sprite = current;
    //    oldWeaponImageUI.sprite = old;

    //    if (weaponImageUI.sprite != null)
    //    {
    //        foreach (Image image in weaponSelectedOverlays)
    //        {
    //            image.gameObject.SetActive(true);
    //        }
    //    }
    //    else
    //    {
    //        foreach (Image image in weaponSelectedOverlays)
    //        {
    //            image.gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void setWeaponOneUI(Sprite image)
    {
        if (image == null)
        {
            weaponOneImageUI.gameObject.SetActive(false);
        }
        else
        {
            weaponOneImageUI.sprite = image;
        }
    }

    public void setWeaponTwoUI(Sprite image)
    {
        if (image == null)
        {
            weaponTwoImageUI.gameObject.SetActive(false);
        }
        else
        {
            weaponTwoImageUI.sprite = image;
        }
    }

    // updates the consumables UI
    public void updateConsumablesUI(Sprite image, float amount)
    {
        if (image == null)
        {
            consumableImageUI.gameObject.SetActive(false);
            consumableAmountUI.text = "x" + amount;
            return;
        }

        consumableImageUI.gameObject.SetActive(true);
        consumableImageUI.sprite = image;
        consumableAmountUI.text = "x" + amount;
    }

    //update the collectible UI
    public void updateCollectibleUI()
    {
        // find with tag is evil
        Inventory playerInventory = GameObject.FindWithTag("Player").GetComponentInParent<Inventory>();

        collectibleAmoutUI.text = "" + playerInventory.collectibleCount;
    }

    public void updateLivesUI(int lives)
    {
        // find with tag is evil
        //PlayerManager lifeAmount = GameObject.FindWithTag("Player").GetComponentInParent<PlayerManager>();

        livesAmountUI.text = "" + lives;
    }

    //update the key UI
    public void setKeyUI(int keys)
    {
        keyAmountUI.text = "" + keys;
    }

    public void setObjectiveText(string text)
    {
        objectivesText.text = text;
    }

    public void setWeaponUseUI(float amount, bool active = true)
    {
        weaponUseAmountUI.text = "x" + amount;
        weaponUseAmountUI.transform.parent.gameObject.SetActive(active);
    }

    public void setLoadingProgress(float value)
    {
        if (loadingSlider != null)
        {
            //print("setting loading slider value: " + value);
            loadingSlider.transform.parent.gameObject.SetActive(true);
            loadingSlider.value = value;
        }
        else
        {
            print("no loading slider is present");
        }
    }
}
