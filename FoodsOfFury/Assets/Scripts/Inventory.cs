using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class manages a inventory of gameobjects ('Item's) using a dictionary that
//              holds 'InventoryList's (which holds the gameObjects).
//
// TODO: Overall Development, add constructor for inpector 
//       initilization (Item array paramer and set lists)
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

    // return current item reference in list from inventory
    public GameObject get(ItemType type)
    {
        InventoryList temp;
        if (inventory.TryGetValue(type, out temp))
        {
            return temp.get(); // return item
        }
        return null; // no item to return
    }

    // add item to inventory
    public bool add(Item item)
    {
        if (itemMaximums.getMax(item.type) == 0)
        {
            print("Can't pickup this item!");
            return false;
        }

        item.gameObject.SetActive(false);
        InventoryList temp;

        if (inventory.TryGetValue(item.type, out temp))
        {
            GameObject copy = Instantiate(item.gameObject, transform); // create copy (also parent to this transform)
            copy.name = item.type.ToString();
            if (!temp.add(copy)) // add copy to existing list
            {
                //temp.exchange(item); // exchange item (TODO: need to test)
            }

            return true;
        }
        else
        {
            ItemType type = item.type;
            inventory.Add(type, new InventoryList(itemMaximums.getMax(type))); // create new list and add to inventory
            return add(item);                                                  // call add again to add item to newly added list
        }
    }

    // remove item from inventory
    public void remove(ItemType type)
    {
        InventoryList temp;
        if (inventory.TryGetValue(type, out temp))
        {
            GameObject reference = temp.get();  // get reference from list
            drop(reference, type);              // drop new copy
            temp.delete();                      // delete reference from list
            Destroy(reference);                 // destroy original item gameobject

            if (temp.isEmpty())
            {
                inventory.Remove(type); // remove empty list from dictionary
            }
        }
    }

    // drop item in world
    private void drop(GameObject item, ItemType type)
    {
        GameObject copy = Instantiate(item);
        copy.transform.position = transform.position + new Vector3(0, 0, 5);
        copy.transform.rotation = transform.rotation;
        copy.name = type.ToString();
        copy.SetActive(true);
    }

    public void printList(ItemType type)
    {
        InventoryList temp;
        if (inventory.TryGetValue(type, out temp))
        {
            temp.printList();
        }
        else
        {
            print(type + " list does not exist");
        }
    }
}
