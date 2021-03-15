using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Acts as a health consumable that modifies a gameobjects 'Health' amount
//----------------------------------------------------------------------------------------

public class Consumable : MonoBehaviour
{
    public float healthAmount = 1.0f;

    public bool use(GameObject obj)
    {
        return gameObject.GetComponentInParent<Health>().add(healthAmount);
    }
}
