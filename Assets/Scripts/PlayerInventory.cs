using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : IInventory
{

    public int capacity { get { return items.Length; } }

    public Item[] items { get; set; }


    // Constructor
    public PlayerInventory (int capacity)
    {
        items = new Item[capacity];
    }


    public bool AddItem (Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) {
                items[i] = item;
                return true;
            }
        }
        return false;
    }


    public bool AddItemAt (Item item, Slot slot)
    {
        if (items[slot.id] == null) {
            items[slot.id] = item;
            return true;
        }
        return false;
    }


    public bool RemoveItem (Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item) {
                items[i] = null;
                return true;
            }
        }
        return false;
    }


    public bool RemoveItemAt (Slot slot)
    {
        if (items[slot.id] != null) {
            items[slot.id] = null;
            return true;
        }
        return false;
    }


    public bool RemoveItemAt (int id)
    {
        if (items[id] != null) {
            items[id] = null;
            return true;
        }
        return false;
    }


    public Item GetItemAt (Slot slot)
    {
        if (items[slot.id] != null) {
            return items[slot.id];
        }
        return null;
    }


    public bool ItemExists (Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item) {
                return true;
            }
        }
        return false;
    }


    public bool ItemExistsAt (Slot slot)
    {
        if (items[slot.id] != null) {
            return true;
        }
        return false;
    }


    public void ResizeInventory(int newSize)
    {
        if (newSize < items.Length)
        {
            Compact(newSize);
            items = Truncate(items, newSize);
        }
        else {
            Item[] resizeItems = new Item[newSize];
            Array.Copy(items, resizeItems, items.Length);
            items = resizeItems;
        }
    }


    public void Compact(int newSize)
    {
        for (int c = items.Length-1; c > 0; c--)
        {
            if (items[c] != null)
            {
                for (int i = 0; i < newSize; i++)
                {
                    if (items[i] == null && i < c)
                    {
                        items[i] = items[c];
                        items[c] = null;
                        break;
                    }
                }
            }
        }
    }


    public Item[] Truncate(Item[] items, int newSize)
    {
        Item[] resizeItems = new Item[newSize];

        for (int i = 0; i < items.Length; i++)
        {
            if (i > newSize-1 && items[i] != null)
            {
                Drag.Instance.CreateWorldItem(items[i], Vector3.zero);
                RemoveItemAt(i);
            }
            else if (i < newSize && items[i] != null)
            {
                resizeItems[i] = items[i];
            }
        }

        return resizeItems;
    }

}