using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Serves as an easy way to get item type
//----------------------------------------------------------------------------------------

public enum nItemType
{
    None,
    Key,
    StarFruit,
    HealthPickup,
    OnionWeapon,
    KetchupWeapon,
}

public class nItem : MonoBehaviour
{
    public nItemType type;
}