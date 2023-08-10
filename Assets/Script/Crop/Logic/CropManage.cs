using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManage : MonoBehaviour
{
    // Start is called before the first frame update
    public CropData_SO cropData;
    private Grid grid;
    private Transform cropParent;
    private Season season;
    private void OnEnable()
    {
        EventHandler.AfterFade += OnAfterFade;
        EventHandler.PlantSeedEvent += OnPlantSeedEvent;
        EventHandler.GameDayEvent += OnGameDayEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterFade -= OnAfterFade;
        EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
        EventHandler.GameDayEvent -= OnGameDayEvent;
    }

    private void OnGameDayEvent(int day, Season Season)
    {
        season = Season;
    }

    private void OnAfterFade(string obj)
    {
        grid = FindObjectOfType<Grid>();
        cropParent = GameObject.FindWithTag("CropParent").GetComponent<Transform>();
    }
    private void OnPlantSeedEvent(int itemID, TileDetails details)
    {
        CropDetails cropDetails = GetCropDetailsForID(itemID);
        if(cropDetails != null && IsSeasonActive(cropDetails) && details.seeItemID == -1)//种下农作物
        {
            details.seeItemID = cropDetails.seedItemID;
            details.growthDays = 0;
            //显示农作物
            SeedPlant(cropDetails, details);
        }
        else if(details.seeItemID == -1)//刷新地图
        {
            //显示种子
            SeedPlant(cropDetails, details);
        }
    }

   /// <summary>
   /// 通过id获得种子的信息
   /// </summary>
   /// <param name="id">种子ID</param>
   /// <returns></returns>
   private CropDetails GetCropDetailsForID(int id)
    {
        return cropData.cropDetailsList.Find(c => c.seedItemID == id);
    }
    /// <summary>
    /// 判断种子在当前季节是否能够播种
    /// </summary>
    /// <param name="cropDetails">种子的信息</param>
    /// <returns></returns>
    private bool IsSeasonActive(CropDetails cropDetails)
    {
        for(int i = 0;i<cropDetails.seasons.Length;i++)
        {
            if (cropDetails.seasons[i] == season)
            {
                return true;
            }
        }
        return false;
    }


    private void SeedPlant(CropDetails cropDetails,TileDetails tileDetails)
    { 
        int growthLenth = cropDetails.growthDays.Length;
        int currentGrowth =0;
        int dayCount = cropDetails.TotalGrowthDays;
        for(int i = growthLenth-1;i>=0;i--)
        {
            if(dayCount <= tileDetails.growthDays)
            {
                currentGrowth = i;
                break;
            }
            dayCount -= cropDetails.growthDays[i];
        }
        GameObject CropPrefab = cropDetails.growthPrefabs[currentGrowth];
        Sprite CropSprite = cropDetails.growthSprite[currentGrowth];
        Vector3 SeedPos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);

        GameObject cropInMap = Instantiate(CropPrefab,SeedPos,Quaternion.identity,cropParent);
        cropInMap.GetComponentInChildren<SpriteRenderer>().sprite = CropSprite;
    }
}
