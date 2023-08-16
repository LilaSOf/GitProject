using MFarm.GridMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CropGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public int SeedID;
    public int GrowthDays;
    private Grid grid;
   private void Awake()
    {
        grid =FindObjectOfType<Grid>();
    }
    private void OnEnable()
    {
        EventHandler.GeneratCropEvent += GeneralCropMap;
    }
    private void OnDisable()
    {
        EventHandler.GeneratCropEvent -= GeneralCropMap;
    }
    // Update is called once per frame
    private void GeneralCropMap()
    {
       Vector3Int pos = grid.WorldToCell(transform.position);

        if(SeedID!=0)
        {
            var tile = GridMapManage.Instance.GetKeyDict(pos);
            if(tile == null) 
            {
                tile = new TileDetails();
            }
            tile.daysSinceWatered = -1;
            tile.growthDays = GrowthDays;
            tile.seeItemID = SeedID;
            tile.gridX = pos.x;
            tile.gridY = pos.y;
            GridMapManage.Instance.UpdateTiledetailsDect(tile);
        }
    }
}
