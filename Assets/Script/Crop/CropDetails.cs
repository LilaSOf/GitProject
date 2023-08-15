using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("��ͬ�׶���Ҫ������")]
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

    [Header("��ͬ���ǽ׶���ƷPrefab")]
    public GameObject[] growthPrefabs;

    [Header("��ͬ�׶ε�ͼƬ")]
    public Sprite[] growthSprite;

    [Header("����ֲ����")]
    public Season[] seasons;


    [Space]
    [Header("�ո��")]
    public int[] harvestToolItemID;

    [Header("ÿ�ֹ���ʹ�ô���")]
    public int[] requireActionCount;
    [Header("ת������ƷID")]
    public int transferItemID;

    [Space]
    [Header("�ո��ʵ��Ϣ")]
    public int[] producedItemID;
    public int[] producedMinAmount;
    public int[] producedMaxAmount;
    public Vector2 spawnRadius;

    [Header("�ٴ�����ʱ��")]
    public int daysToRegrow;
    public int regrowTimes;

    [Header("Options")]
    public bool generateAtPlayerPosition;
    public bool hasAniamtion;
    public bool hasParticalEffect;
    //TODO:��Ч����Ч��
    public ParticleType particleType;
    public Vector3 particleEffectPos;

   /// <summary>
   /// �жϹ����Ƿ��ܹ��ջ�
   /// </summary>
   /// <param name="ItemID">���ߵ�ID</param>
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
    /// �жϹ����ջ�Ĵ���
    /// </summary>
    /// <param name="ItemID">����ID</param>
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
