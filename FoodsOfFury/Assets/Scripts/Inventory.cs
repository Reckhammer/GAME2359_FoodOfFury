using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages a inventory of 'Item's  using a dictionary that
//              holds 'InventoryList' (which hold items)
//
// TODO: Overall Development, Change creation of world objects from manual to prefabs,
//       add constructor for inpector Initilization (Item array paramer and set lists)
//----------------------------------------------------------------------------------------

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public struct Maximums
    {
        public int maxWeaponItems;
        public int maxHealthItems;

        public int getMax(ItemType type)
        {
            switch (type)
            {
                case ItemType.Weapon:
                    return maxWeaponItems;
                case ItemType.Health:
                    return maxHealthItems;
                default:
                    return -1;
            }
        }
    };

    public Maximums itemMaximums; // struct to hold maximums of 'ItemType's (need to add to this as ItemType grows)

    private Dictionary<ItemType, InventoryList> inventory = new Dictionary<ItemType, InventoryList>(); // Dictionary of 'ItemLists'

    // use item
    //public void use(ItemType type)
    //{

    //}

    // return current item in list from inventory
    public Item get(ItemType type)
    {
        InventoryList temp;
        if (inventory.TryGetValue(type, out temp))
        {
            return temp.get(); // return item
        }
        return null; // no item to return
    }

    // add item to inventory
    public void add(Item item)
    {
        InventoryList temp;

        if (inventory.TryGetValue(item.Type(), out temp))
        {
            if (!temp.add(item)) // add item to existing list
            {
                //temp.exchange(item); // exchange item (TODO: need to test)
            }
        }
        else
        {
            ItemType type = item.Type();
            inventory.Add(type, new InventoryList(itemMaximums.getMax(type))); // create new list and add to inventory
            add(item); // call add again to add item to newly added list
        }
    }

    // remove item from inventory
    public void remove(ItemType type)
    {
        InventoryList temp;
        if (inventory.TryGetValue(type, out temp))
        {
            drop(temp.get());
            temp.delete(); // delete item

            if (temp.isEmpty())
            {
                inventory.Remove(type); // remove empty list from dictionary
            }
        }
    }

    // drop item in world (TODO: create from prefab)
    private void drop(Item item)
    {
        GameObject obj = new GameObject();
        obj.transform.position = transform.position + new Vector3(0, 0, 5);
        obj.transform.rotation = transform.rotation;

        BoxCollider col = obj.AddComponent<BoxCollider>();
        col.isTrigger = true;

        switch (item.Type())
        {
            case ItemType.Weapon:
                obj.AddComponent<Weapon>();
                obj.AddComponent<Pickupable>();
                break;
            case ItemType.Health:
                // obj.AddComponent<HealthItem>();
                break;
            default:
                break;
        }
    }
}
