using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    // Start is called before the first frame update
    public CropDetails cropDetails;

   [SerializeField] private int harvestCropCount;
    /// <summary>
    /// 执行收获的逻辑
    /// </summary>
    /// <param name="toolDetails">地图网格的信息</param>
    public void ProcessToolAction(ItemDetails toolDetails)
    {   
        //计算工具使用次数
        int requireToolAoumt = cropDetails.RequireActionAoumt(toolDetails.ID);
        if (requireToolAoumt == -1) return;

        //判断是否有动画

        //点击计数器
        if(harvestCropCount < requireToolAoumt)
        {
            harvestCropCount++;
            //播放粒子
            //播放声音
        }
      if(harvestCropCount >= requireToolAoumt)
        {
            if(cropDetails.generateAtPlayerPosition)
            {
                   SpawnHarvestItem();
            }
        }
    }

    public void SpawnHarvestItem()
    {
      
        for(int i = 0;i<cropDetails.producedItemID.Length;i++)
        {
            int amountToPrduce;
            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                amountToPrduce = cropDetails.producedMinAmount[i];
            }
            else
            {
                amountToPrduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
             
            }
            for (int j = 0; j < amountToPrduce; j++)
            {
                if(cropDetails.generateAtPlayerPosition)
                    EventHandler.CallHarvestInPlayerPostion(cropDetails.producedItemID[i]);
            }
        }
    }
}
