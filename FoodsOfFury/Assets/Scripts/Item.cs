using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Interface for 'Item's.
//
// TODO: Overall development
//----------------------------------------------------------------------------------------

public interface Item
{
    GameObject gameObject { get; }
    ItemType type { get; }
    //Image imageIcon { get; } // return image icon
    //void Use();         // use the item
}
