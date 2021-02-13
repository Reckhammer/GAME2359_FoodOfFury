using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class acts as a manager for a list of 'GameObject's.
//
// TODO: add infinite maximum, test commented methods, add constructor to allow for 
//       initialization from Item array
//----------------------------------------------------------------------------------------

public class InventoryList
{
    public int max; // max entries to list

    private List<GameObject> list = new List<GameObject>(); // list of items
    private int current;                                    // current index of item

    // Constructor
    public InventoryList(int max)
    {
        this.max = max;
    }

    // returns the current item from the list
    public GameObject get()
    {
        return list[current];
    }

    // adds to list if not at max (return true if successful)
    public bool add(GameObject item)
    {
        if (list.Count != max)
        {
            list.Add(item);
            return true;
        }
        return false;
    }

    // return next 'Item' in list and update 'current'
    //public Item next()
    //{
    //    current = (current++ != max) ? current++ : 0; // update current to next index (loops back to begging '0')
    //    return list[current];
    //}

    // return previous 'Item' in list and update 'current'
    //public Item previous()
    //{
    //    current = (current-- != 0) ? current-- : max; // update 'current' to previous index (loops back to 'max')
    //    return list[current];
    //}

    // deletes from list at index (current if none specified)
    public void delete(int index = -1)
    {
        if (list.Count == 0)
        {
            return; // return if empty
        }

        if (index == -1)
        {
            list.RemoveAt(current); // remove current item
            if (current == max)
            {
                current--;
            }
        }
        else
        {
            list.RemoveAt(index); // remove at index
            if (index == max)
            {
                current--;
            }
        }
    }

    // returns false if empty
    public bool isEmpty()
    {
        if (list.Count == 0)
        {
            return true;
        }
        return false;
    }

    // prints list in debugger
    public void printList()
    {
        int amount = 1;

        foreach (GameObject item in list)
        {
            Debug.Log(item + " : " + amount);
            amount++;
        }
    }
}