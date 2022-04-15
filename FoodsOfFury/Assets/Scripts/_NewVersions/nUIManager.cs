using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Manages main UI
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
    public Text starFruitAmountUI;          // reference to star fruit amount text
    public Text keyAmountUI;                // reference to key amount text
    public Text objectivesText;             // reference to objectives text
    public Text weaponUseAmountUI;          // reference to weapon use amount (ex. ketchup shots left)
    public Text livesAmountUI;
    public Slider loadingSlider;            // reference to loading slider for loading screen
    public Sprite[] commonIcons;            // reference to icons to commonly used icons

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

    // set weapon one UI
    public void setWeaponOneUI(nItemType type)
    {
        if (type == nItemType.None)
        {
            weaponOneImageUI.gameObject.SetActive(false);
        }
        else
        {
            weaponOneImageUI.sprite = commonIcons[getIconFromType(type)];
        }
    }

    // set weapon two UI
    public void setWeaponTwoUI(nItemType type)
    {
        if (type == nItemType.None)
        {
            weaponTwoImageUI.gameObject.SetActive(false);
        }
        else
        {
            weaponTwoImageUI.sprite = commonIcons[getIconFromType(type)];
        }
    }

    // set the consumable UI
    public void setConsumablesUI(nItemType type, float amount)
    {
        if (type == nItemType.None)
        {
            consumableImageUI.gameObject.SetActive(false);
            consumableAmountUI.text = "x" + amount;
            return;
        }

        consumableImageUI.gameObject.SetActive(true);
        consumableImageUI.sprite = commonIcons[getIconFromType(type)];
        consumableAmountUI.text = "x" + amount;
    }

    // set the star fruit UI
    public void setStarFruitUI(int amount)
    {
        starFruitAmountUI.text = "" + amount;
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

    // returns index for commonIcons from type
    private int getIconFromType(nItemType type)
    {
        switch (type)
        {
            case nItemType.HealthPickup:
                return 0;
            case nItemType.OnionWeapon:
                return 1;
            case nItemType.KetchupWeapon:
                return 2;
            default:
                return -1;
        }
    }
}
