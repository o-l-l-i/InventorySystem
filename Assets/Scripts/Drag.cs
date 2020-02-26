using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Drag : MonoBehaviour
{

    Image image;
    Text itemText;

    public bool textVisible { get { if (itemText.enabled) return true; else return false; }}

    public static Item handledItem { get; set; }
    public static Slot sourceSlot { get; set; }
    public static Slot currentSlot { get; set; }
    public static bool isDragging { get; set; }
    public static GameObject currentWorldItem { get; set; }

    public delegate void SlotUpdateAction();
    public static event SlotUpdateAction OnSlotsUpdated;

    private static Drag _instance;
    public static Drag Instance { get { return _instance; } }


    void Awake ()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        image = this.GetComponent<Image>();
        itemText = this.GetComponentInChildren<Text>();
    }


    void Start ()
    {
        UnhandleItem();
    }


    void Update ()
    {
        transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && currentSlot == null)
            BeginWordDrag();

        else if (Input.GetMouseButtonUp(0) && handledItem != null && currentSlot != null && currentSlot.item == null)
            WorldToEmptySlotDrag();

        else if (Input.GetMouseButtonUp(0) && handledItem != null && currentSlot != null && currentSlot.item != null)
            WorldToFullSlotDrag();

        else if (Input.GetMouseButtonUp(0) && handledItem != null && currentSlot == null)
           WorldToWorldDrag();
    }


    public void BeginWordDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayhit = new RaycastHit();

        if(Physics.Raycast(ray,out rayhit))
        {
            currentWorldItem = rayhit.transform.gameObject;
            if (currentWorldItem.GetComponent<ItemContainer>())
            {
                var itemContainer = currentWorldItem.GetComponent<ItemContainer>();
                HandleItem(null, itemContainer.item);
            }
        }
    }

    public void WorldToEmptySlotDrag()
    {
        currentSlot.owner.AddItemAt(handledItem, currentSlot);
        OnSlotsUpdated();
        UnhandleItem();
        DestroyWorldItem(currentWorldItem);
    }


    public void WorldToFullSlotDrag()
    {
        UnhandleItem();
    }


    public void WorldToWorldDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayhit = new RaycastHit();

        Vector3 position = currentWorldItem.transform.position;

        if(Physics.Raycast(ray, out rayhit)) {
            position = rayhit.point;
        }

        MoveWorldItem(currentWorldItem, position);
        UnhandleItem();
    }


    public void EndDrag()
    {
        if (sourceSlot == currentSlot)
        {
            UnhandleItem();
        }

        else if (currentSlot == null)
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit rayhit = new RaycastHit();

            if(Physics.Raycast(ray, out rayhit)) {
                CreateWorldItem(sourceSlot.item, rayhit.point);
                sourceSlot.owner.RemoveItemAt(sourceSlot);
                UnhandleItem();
                OnSlotsUpdated();
            }
            else {
                UnhandleItem();
            }
        }

        else if (currentSlot != null)
        {
            if (currentSlot != sourceSlot &&
                currentSlot.item == null)
            {
                currentSlot.owner.AddItemAt(handledItem, currentSlot);
                sourceSlot.owner.RemoveItemAt(sourceSlot);
                UnhandleItem();
                OnSlotsUpdated();
            }
            else if (currentSlot != sourceSlot &&
                     currentSlot.item != null)
            {
                SwapItems(sourceSlot, currentSlot);
                UnhandleItem();
                OnSlotsUpdated();
            }
        }
    }


    public void HandleItem (Slot slot, Item item)
    {
        sourceSlot = slot;
        handledItem = item;
        isDragging = true;
        image.sprite = item.icon;
        image.enabled = true;
        ShowItemText(item);
    }


    public void UnhandleItem () // There can be only one!
    {
        sourceSlot = null;
        handledItem = null;
        isDragging = false;
        image.sprite = null;
        image.enabled = false;
        if (currentSlot == null || currentSlot.item == null) {
            HideItemText();
        } else {
            ShowItemText(currentSlot.item);
        }
    }


    public void SwapItems (Slot source, Slot target)
    {
        var sourceInventory = source.owner;
        var targetInventory = target.owner;
        Item temp = currentSlot.item; // Copy target to temp
        targetInventory.RemoveItemAt(currentSlot); // Clear target
        targetInventory.AddItemAt(sourceSlot.item, currentSlot); // copy source to target
        sourceInventory.RemoveItemAt(sourceSlot); // Clear source
        sourceInventory.AddItemAt(temp, sourceSlot); // copy temp to source
    }


    public void ShowItemText (Item item)
    {
        itemText.text = item.itemName;
        itemText.enabled = true;
    }


    public void HideItemText ()
    {
        itemText.text = "";
        itemText.enabled = false;
    }


    public void CreateWorldItem (Item item, Vector3 position)
    {
        var obj = GameObject.Instantiate(item.representation, position, Quaternion.identity);
        obj.name = item.representation.name;
    }


    public void DestroyWorldItem (GameObject worldItem)
    {
        Destroy(worldItem);
    }


    public void MoveWorldItem (GameObject worldItem, Vector3 position)
    {
        worldItem.transform.position = position;
    }

}