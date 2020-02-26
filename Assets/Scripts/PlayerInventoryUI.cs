using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventoryUI : MonoBehaviour
{
    [Header("Inventory Parameters")]
    public int inventoryInitSize = 12;
    int inventoryCurrentSize;

    public PlayerInventory playerInventory;

    [Header("Created Slots")]
    public List<Slot> slots = new List<Slot>();

    [Header("Components")]
    [ReadOnly] public CanvasGroup canvasGroup;

    [Header("Slot Bezel Image")]
    public Sprite slotGraphic;

    bool inventoryActive;


    void OnEnable ()
    {
        Drag.OnSlotsUpdated += UpdateSlotViews;
    }


    void OnDisable ()
    {
        Drag.OnSlotsUpdated -= UpdateSlotViews;
    }


    void Awake ()
    {
        canvasGroup = this.transform.parent.GetComponent<CanvasGroup>();
        playerInventory = new PlayerInventory(inventoryInitSize);
        inventoryCurrentSize = inventoryInitSize;
    }


    void Update ()
    {
        if (inventoryCurrentSize != inventoryInitSize)
        {
            if (inventoryInitSize <= 0)
                inventoryInitSize = 1;

            inventoryCurrentSize = inventoryInitSize;
            playerInventory.ResizeInventory(inventoryInitSize);
            DestroySlots();
            CreateSlots();
            UpdateSlotViews();
        }
    }


    void Start ()
    {
        CreateSlots();
        PopulateSlots(4);
        UpdateSlotViews();
        DeactivateInventory();
        inventoryActive = false;
    }


    public void ToggleInventory ()
    {
        inventoryActive = !inventoryActive;
        if (inventoryActive)
            ActivateInventory();
        else
            DeactivateInventory();
    }


    void ActivateInventory ()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }


    void DeactivateInventory ()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    void CreateSlots ()
    {
        for (int i = 0; i < playerInventory.capacity; i++)
        {
            GameObject slotObj = new GameObject("slot_" + i);

            GameObject slotFrame = new GameObject("slotFrame_" + i);
            var slotFrameImage = slotFrame.AddComponent<Image>();
            slotFrameImage.color = new Color(0.5f,0.5f,0.5f);
            if (slotGraphic != null)
                slotFrameImage.sprite = slotGraphic;

            var slot = slotObj.AddComponent<Slot>();
            slot.id = i;
            slot.owner = playerInventory;
            slots.Add(slot);

            var image = slotObj.AddComponent<Image>();
            image.rectTransform.sizeDelta = new Vector2(75f, 75f);
            slot.visual = image;

            slotFrame.transform.SetParent(this.transform);

            slot.transform.SetParent(slotFrame.transform);
        }
    }


    void DestroySlots ()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i].transform.parent.gameObject);
        }

        slots.Clear();
    }


    void PopulateSlots (int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (ItemLibrary.Instance.itemPool[i] != null)
                playerInventory.items[i] = ItemLibrary.Instance.itemPool[i];
        }
    }


    public void UpdateSlotViews ()
    {
        for (int i = 0; i < playerInventory.capacity; i++)
        {
            if (playerInventory.items[i] != null) {
                if (playerInventory.items[i].icon != null) {
                    slots[i].visual.sprite = playerInventory.items[i].icon;
                    slots[i].visual.color = new Color(1f,1f,1f);
                }
            }
            else {
                slots[i].visual.sprite = null;
                slots[i].visual.color = new Color(.75f,.75f,.75f);
            }
        }
    }

}