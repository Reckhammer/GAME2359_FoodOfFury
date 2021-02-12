using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Simple 'Weapon' class that implements the 'Item' interface
//
// TODO: Overall development
//----------------------------------------------------------------------------------------

public class Weapon : MonoBehaviour, Item
{
    // some varialbes here

    // return the type
    public ItemType Type()
    {
        return ItemType.Weapon;
    }

    // some methods here
}
