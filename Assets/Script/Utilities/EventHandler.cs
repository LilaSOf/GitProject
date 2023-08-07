using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.SearchService;

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

    public static event Action<string, Vector3> TranslationEvent;
    public static void CallTranslationEvent(string newSceneName,Vector3 targetPos)
    {
        TranslationEvent?.Invoke(newSceneName,targetPos);
    }

    public static event Action<string> BeforeFade;
    public static void CallBeforeFade(string sceneName)
    {
        BeforeFade?.Invoke(sceneName);
    }

    public static event Action<string> AfterFade;
    public static void CallAfterFade(string sceneName)
    {
        AfterFade?.Invoke(sceneName);
    }

    public static event Action<ItemDetails, bool> ItemSelectEvent;
    public static void CallItemSelectEvent(ItemDetails itemDetails,bool isSelect)
    {
        ItemSelectEvent?.Invoke(itemDetails,isSelect);
    }

    public static event Action<string> SceneNameTransfer;
    public static void CallSceneNameTransfer(string sceneName)
    {
        SceneNameTransfer.Invoke(sceneName);
    }

    public static Action<Vector3, ItemDetails> MouseClickEvent;
    public static void CallMouseClickEvent(Vector3 pos,ItemDetails itemDetails)
    {
        MouseClickEvent?.Invoke(pos,itemDetails);
    }
    public static Action<Vector3, ItemDetails> AfterPlayerAnimation;
    public static void CallAfterPlayerAnimation(Vector3 pos,ItemDetails itemDetails)
    {
        AfterPlayerAnimation.Invoke(pos,itemDetails);
    }
}
