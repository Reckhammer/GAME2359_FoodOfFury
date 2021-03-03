using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class acts as a game manager singleton. It currently manages the 
//              Health Bar UI
//
// TODO: Overall Development
//----------------------------------------------------------------------------------------

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; } // GameController instance

    public HealthBar healthBar; // reference to player health bar

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
}
