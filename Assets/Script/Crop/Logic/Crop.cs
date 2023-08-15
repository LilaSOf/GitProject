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
    /// 执行收获的逻辑
    /// </summary>
    /// <param name="toolDetails">地图网格的信息</param>
    public void ProcessToolAction(ItemDetails toolDetails,TileDetails tile)
    {
        tileDetails = tile;
        animator = GetComponentInChildren<Animator>();
        //计算工具使用次数
        int requireToolAoumt = cropDetails.RequireActionAoumt(toolDetails.ID);
        if (requireToolAoumt == -1) return;

        //判断是否有动画
     
        //点击计数器
        if (harvestCropCount < requireToolAoumt)
        {
            harvestCropCount++;
            //播放动画
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
            //播放粒子
            if (cropDetails.hasParticalEffect) EventHandler.CallParticleEffectEvent(cropDetails.particleType,transform.position+ cropDetails.particleEffectPos);
            //播放声音
        }
        if (harvestCropCount >= requireToolAoumt)
        {
            if(cropDetails.generateAtPlayerPosition)
            {
                   SpawnHarvestItem();
            }
            else if(cropDetails.hasAniamtion)
            {
                if (Player_Trans.position.x > transform.position.x)
                {
                    animator.SetTrigger("FallLeft");
                }
                else
                {
                    animator.SetTrigger("FallRight");
                }
                StartCoroutine(HarvestAfterAnimation());
            }
            else
            {
                SpawnHarvestItem();
            }
        }
    }
    private IEnumerator HarvestAfterAnimation()
    {
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("END"))
        {
            yield return null;
        }
        SpawnHarvestItem();
        if(cropDetails.transferItemID>0)
        {
            CreatCropInMap();
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
                else
                {
                    float dirX = transform.position.x > Player_Trans.position.x?1:-1;
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX), transform.position.y
                        + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y));
                    EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                }
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
    private void CreatCropInMap()
    {
       tileDetails.growthDays =0;
        tileDetails.seeItemID = cropDetails.transferItemID;
        tileDetails.daysSinceLasterHarvest=-1;
        EventHandler.CallRefreshMap();
    }
}
