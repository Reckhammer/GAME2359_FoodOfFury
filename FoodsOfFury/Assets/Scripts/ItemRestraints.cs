using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Health
};

[System.Serializable]
public class ItemRestraints
{
    public int maxWeapons = 0;
    public int maxHealth = 0;

    public int getMax(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                return maxWeapons;
            case ItemType.Health:
                return maxHealth;
            default:
                return 0;
        }
    }
}