using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("不同阶段需要的天数")]
    public int[] growthDays;
    public int TotalGrowthDays 
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays) 
            {
                amount += days;
            }
            return amount;
        }
    }

    [Header("不同上涨阶段物品Prefab")]
    public GameObject[] growthPrefabs;

    [Header("不同阶段的图片")]
    public Sprite[] growthSprite;

    [Header("可种植季节")]
    public Season[] seasons;


    [Space]
    [Header("收割工具")]
    public int[] harvestToolItemID;

    [Header("每种工具使用次数")]
    public int[] requireActionCount;
    [Header("转换新物品ID")]
    public int transferItemID;

    [Space]
    [Header("收割果实信息")]
    public int[] producedItemID;
    public int[] producedMinAmount;
    public int[] producedMaxAmount;
    public Vector2 spawnRadius;

    [Header("再次生长时间")]
    public int daysToRegrow;
    public int regrowTimes;

    [Header("Options")]
    public bool generateAtPlayerPosition;
    public bool hasAniamtion;
    public bool hasParticalEffect;
    //TODO:特效，音效等
    public ParticleType particleType;
    public Vector3 particleEffectPos;

   /// <summary>
   /// 判断工具是否能够收获
   /// </summary>
   /// <param name="ItemID">工具的ID</param>
   /// <returns></returns>
   public bool CheckToolAvailable(int ItemID)
    {
        for(int i = 0;i<harvestToolItemID.Length;i++)
        {
            if(ItemID == harvestToolItemID[i])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 判断工具收获的次数
    /// </summary>
    /// <param name="ItemID">工具ID</param>
    /// <returns></returns>
    public int RequireActionAoumt(int ItemID)
    {
        for(int i=0;i<harvestToolItemID.Length;i++)
        {
            if(ItemID == harvestToolItemID[i])
            {
                return requireActionCount[i];
            }
          
        }
        return -1;
    }
}
