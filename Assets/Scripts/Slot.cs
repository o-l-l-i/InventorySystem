using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public IInventory owner;
    public int id { get; set; }
    public Image visual { get; set; }


    public Slot(IInventory owner)
    {
        this.owner = owner;
    }


    public Item item
    {
        get {
            if (owner.items[id] != null)
                return owner.items[id];
            else
                return null;
        }
    }


    public void OnBeginDrag (PointerEventData eventData)
    {
        if (owner.items[id] == null) {
            return;
        }

        Drag.Instance.HandleItem(this, item);
    }


    public void OnDrag (PointerEventData eventData)
    {

    }


    public void OnEndDrag (PointerEventData eventData)
    {
        Drag.Instance.EndDrag();
    }


    public void OnPointerDown (PointerEventData eventData)
    {
        if (item != null)
            Drag.Instance.HandleItem(this, item);
    }


    public void OnPointerUp (PointerEventData eventData)
    {
        if (!Drag.isDragging)
            Drag.Instance.UnhandleItem();
    }


    public void OnPointerEnter (PointerEventData eventData)
    {
        Drag.currentSlot = this;

        if (item != null)
            Drag.Instance.ShowItemText(item);
    }


    public void OnPointerExit (PointerEventData eventData)
    {
        Drag.currentSlot = null;

        if (!Drag.isDragging)
            Drag.Instance.HideItemText();
    }

}