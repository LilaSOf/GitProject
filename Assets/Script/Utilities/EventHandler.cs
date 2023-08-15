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

    public static event Action<int, Vector3> DropItemEvent;
    public static void CallDropItemEvent(int id,Vector3 pos)
    {
        DropItemEvent?.Invoke(id, pos);
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
    public static event Action<int,Season> GameDayEvent;
    public static void CallGameDayEvent(int Days,Season season)
    {
        GameDayEvent.Invoke(Days, season);
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


    public static Action<Vector3, ItemDetails> ExcuteActionAfterAnimation;
    public static void CallExcuteActionAfterAnimation(Vector3 pos,ItemDetails itemDetails)
    {
        ExcuteActionAfterAnimation.Invoke(pos,itemDetails);
    }

    public static Action<bool> DropItemInBagEvent;
    public static void CallDropItemInBagEvent(bool CanDrop)
    {
        DropItemInBagEvent?.Invoke(CanDrop);
    }

    public static Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int seedID,TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(seedID,tileDetails);
    }

    public static Action<int> HarvestInPlayerPostion;
    public static void CallHarvestInPlayerPostion(int ID)
    {
        HarvestInPlayerPostion?.Invoke(ID);
    }

    public static Action RefreshMap;
    public static void CallRefreshMap()
    {
        RefreshMap?.Invoke();
    }

    public static Action<ParticleType, Vector3> ParticleEffectEvent;
    public static void CallParticleEffectEvent(ParticleType particleType,Vector3 pos)
    {
        ParticleEffectEvent?.Invoke(particleType,pos);
    }

    public static Action GeneratCropEvent;
    public static void CallGeneratCropEvent()
    {
        GeneratCropEvent?.Invoke();
    }
}
