using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    // Start is called before the first frame update
    public CropDetails cropDetails;

   [SerializeField] private int harvestCropCount;
    public TileDetails tileDetails;
    private Animator animator;

    public bool CanHavest;
    private Transform Player_Trans => FindObjectOfType<Player>().transform;


    private void Update()
    {
        if (tileDetails.growthDays >= cropDetails.TotalGrowthDays)
        {
            CanHavest = true;
        }
        else
        {
            CanHavest = false;
        }
    }

    /// <summary>
    /// ִ���ջ���߼�
    /// </summary>
    /// <param name="toolDetails">��ͼ�������Ϣ</param>
    public void ProcessToolAction(ItemDetails toolDetails,TileDetails tile)
    {
        tileDetails = tile;
        animator = GetComponentInChildren<Animator>();
        //���㹤��ʹ�ô���
        int requireToolAoumt = cropDetails.RequireActionAoumt(toolDetails.ID);
        if (requireToolAoumt == -1) return;

        //�ж��Ƿ��ж���
     
        //���������
        if (harvestCropCount < requireToolAoumt)
        {
            harvestCropCount++;
            //��������
            if (animator != null && cropDetails.hasAniamtion)
            {
                if(Player_Trans.position.x > transform.position.x)
                {
                    animator.SetTrigger("RotateLeft");
                }
                else
                {
                    animator.SetTrigger("RotateRight");
                }
            }
            //��������
        }
      if(harvestCropCount >= requireToolAoumt)
        {
            if(cropDetails.generateAtPlayerPosition)
            {
                   SpawnHarvestItem();
            }
            else
            {
                if (Player_Trans.position.x > transform.position.x)
                {
                    animator.SetTrigger("FallLeft");
                }
                else
                {
                    animator.SetTrigger("FallRight");
                }
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
        if(tileDetails != null)
        {
            if (tileDetails.daysSinceLasterHarvest<=cropDetails.regrowTimes && cropDetails.daysToRegrow > 0)
            {
                tileDetails.daysSinceLasterHarvest++;
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                EventHandler.CallRefreshMap();
            }
            else
            {
                tileDetails.daysSinceLasterHarvest = 0;
                tileDetails.seeItemID = -1;
                tileDetails.daysSinceLasterHarvest = -1;
            }
            Destroy(gameObject);
        }
      

    }
}
