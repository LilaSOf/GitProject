using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class EventHandler 
{
    // Start is called before the first frame update
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI(InventoryLocation location,List<InventoryItem> item)
    {
        UpdateInventoryUI?.Invoke(location, item);
    }
    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int ID,Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }

    public static event Action<ItemDetails, bool> ItemSecletEvent;
    public static void CallItemSecletEvent(ItemDetails itemDetails,bool isSeclet)
    {
        ItemSecletEvent?.Invoke(itemDetails, isSeclet);
    }

    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute,int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int,int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour,int day,int month,int year,Season season)
    {
        GameDateEvent?.Invoke(hour,day, month, year, season);
    }
}
