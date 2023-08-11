using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    // Start is called before the first frame update
    public CropDetails cropDetails;

   [SerializeField] private int harvestCropCount;
    /// <summary>
    /// ִ���ջ���߼�
    /// </summary>
    /// <param name="toolDetails">��ͼ�������Ϣ</param>
    public void ProcessToolAction(ItemDetails toolDetails)
    {   
        //���㹤��ʹ�ô���
        int requireToolAoumt = cropDetails.RequireActionAoumt(toolDetails.ID);
        if (requireToolAoumt == -1) return;

        //�ж��Ƿ��ж���

        //���������
        if(harvestCropCount < requireToolAoumt)
        {
            harvestCropCount++;
            //��������
            //��������
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
