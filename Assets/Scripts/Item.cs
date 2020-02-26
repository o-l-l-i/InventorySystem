using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/Inventory Item", order = 1)]
public class Item : ScriptableObject
{

    public string itemName;
    public string description;
    public float value;
    public Sprite icon;
    public GameObject representation;
    public ItemType itemType;

}