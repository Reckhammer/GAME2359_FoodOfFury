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

    public HealthBar healthBar;     // reference to player health bar
    public Image weaponImageUI;     // reference to weapon image
    public Image consumableImageUI; // reference to consumable image

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
    public void updateWeaponUI(Sprite image)
    {
        if (weaponImageUI == null)
        {
            return;
        }

        weaponImageUI.sprite = image;
    }

    // updates the consumables UI
    public void updateConsumablesUI(Sprite image)
    {
        if (consumableImageUI == null)
        {
            return;
        }

        consumableImageUI.sprite = image;
    }
}
