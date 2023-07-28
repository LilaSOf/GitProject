using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemDetails
{
    public int ID;
    public string itemName;
    public ItemType itemType;
    public string Description;
    public int itemUseRadios;
    public Sprite itemIcon;

    public Sprite itemOnWorldSprite;

    public bool canPickUp;
    public bool canDropped;
    public bool canCarried;

    public int itemPrice;
    [Range(0, 1)]
    public float sellPercentage;
}
[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}