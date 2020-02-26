using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{

    Item[] items { get; }

    bool AddItem (Item item);

    bool AddItemAt (Item item, Slot slot);

    bool RemoveItem (Item item);

    bool RemoveItemAt (Slot slot);

    Item GetItemAt (Slot slot);

    bool ItemExists (Item item);

    bool ItemExistsAt (Slot slot);

}