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
[System.Serializable]
public class AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}
[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
[System.Serializable]
public class SceneItem
{
    public SerializableVector3 position;
    public int itemID;
}
[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}
[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;

    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool NPCObstacle;

    public int daysSinceDug = -1;
    public int daysSinceWatered = -1;
    public int seeItemID = -1;
    public int growthDays = -1;
    public int daysSinceLasterHarvest = -1;
}
