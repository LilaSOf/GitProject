using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.CropM
{
    public class ReapItem : MonoBehaviour
    {
        // Start is called before the first frame update
        public CropDetails cropDetails;
        private Transform Player_Trans => FindObjectOfType<Player>().transform;


        public void InitCrop(int id)
        {
            cropDetails =  CropManage.Instance.GetCropDetailsForID(id);
        }
        public void SpawnHarvestItem()
        {

            for (int i = 0; i < cropDetails.producedItemID.Length; i++)
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
                    if (cropDetails.generateAtPlayerPosition)
                        EventHandler.CallHarvestInPlayerPostion(cropDetails.producedItemID[i]);
                    else
                    {
                        float dirX = transform.position.x > Player_Trans.position.x ? 1 : -1;
                        var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX), transform.position.y
                            + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y));
                        EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                    }
                }
            }
        }
    }
}
