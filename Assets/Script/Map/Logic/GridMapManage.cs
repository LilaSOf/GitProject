using MFarm.CropM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
namespace MFarm.GridMap
{
    public class GridMapManage : Singleton<GridMapManage>
    {
        [Header("获取瓦片")]
        public RuleTile digRuleTile;
        public RuleTile waterRuleTile;
        private Tilemap digTileMap;
        private Tilemap waterTileMap;

        // Start is called before the first frame update
        [Header("获取地图信息")]
        public List<MapData_SO> mapDataList = new List<MapData_SO>();
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();//储存地图信息的字典
        private Dictionary<string,bool> firstLoadDict = new Dictionary<string,bool>();
        private string SceneName;
        private Grid gridMap;

        [Header("时间更新地图组件")]
        private Season currentSeason;
        [Header("设置场景中的杂草")]
        private List<ReapItem> reapItemList;
        private void Start()
        {
            foreach (var mapdataList in mapDataList)
            {
                InitTileDetailsData(mapdataList);
                firstLoadDict[mapdataList.SceneName] = true;
            }
           
        }
        private void OnEnable()
        {
            EventHandler.SceneNameTransfer += OnSceneNameTransfer;
            EventHandler.AfterFade += OnAfterFade;
            EventHandler.ExcuteActionAfterAnimation += OnExcuteActionAfterAnimation;
            EventHandler.GameDayEvent += OnGameDayEvent;
            EventHandler.RefreshMap += RefershMap;
        }

        private void OnDisable()
        {
            EventHandler.SceneNameTransfer -= OnSceneNameTransfer;
            EventHandler.AfterFade -= OnAfterFade;
            EventHandler.ExcuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
            EventHandler.GameDayEvent -= OnGameDayEvent;
            EventHandler.RefreshMap -= RefershMap;
        }


     /// <summary>
     /// 执行实际的逻辑操作
     /// </summary>
     /// <param name="mouseWorldPos">鼠标的世界坐标</param>
     /// <param name="details">物品信息</param>
     private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails details)
        {
            Vector3Int mouseGridPos = gridMap.WorldToCell(mouseWorldPos);
            var currentTile = GetKeyDict(mouseGridPos);
            Crop currentCrop = GetCropOfVector3(mouseWorldPos);
            if (details != null)
            {
                switch (details.itemType)
                {
                    case ItemType.Seed:
                        EventHandler.CallPlantSeedEvent(details.ID, currentTile);
                        break;
                    case ItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.canDig = false;
                        currentTile.daysSinceDug = 0;
                        currentTile.canDropItem = false;
                        //音效
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.canDig = false;
                        currentTile.daysSinceWatered = 0;
                        //音效
                        break;
                    case ItemType.CollectTool:
                        if (currentCrop != null) { currentCrop.ProcessToolAction(details,currentTile); }
                        break;
                    case ItemType.ChopTool:
                    case ItemType.BreakTool:
                       // Debug.Log(currentCrop != null);
                        if (currentCrop != null) { currentCrop.ProcessToolAction(details, currentCrop.tileDetails); }
                        break;
                    case ItemType.ReapTool:
                        int HarvestCount = 0;
                        for (int i = 0;i< reapItemList.Count;i++)
                        {
                            EventHandler.CallParticleEffectEvent(ParticleType.GrassFall, reapItemList[i].transform.position + Vector3.up);
                            reapItemList[i].SpawnHarvestItem();
                            Destroy(reapItemList[i].gameObject);
                            HarvestCount++;
                            if(HarvestCount >=2)
                            {
                                break;
                            }
                        }
                        break;
                }
                  if(currentTile!= null)
                {
                    UpdateTiledetailsDect(currentTile);
                }
            }  
        }

        /// <summary>
        /// 每天刷新一次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="season"></param>
        private void OnGameDayEvent(int day, Season season)
        {
             currentSeason = season;
            foreach(var item in tileDetailsDict)
            {
                if(item.Value.daysSinceWatered>-1)
                {
                    item.Value.daysSinceWatered = -1;
                }
                if(item.Value.daysSinceDug>-1)
                {
                    item.Value.daysSinceDug++;
                }
                if(item.Value.daysSinceDug >5 && item.Value.seeItemID == -1)
                {
                    item.Value.canDig = true;
                    item.Value.daysSinceDug = -1;
                    item.Value.growthDays = -1;
                }
                if(item.Value.seeItemID != -1)
                {
                    item.Value.growthDays++;
                }
            }
            RefershMap();
        }

        private void OnAfterFade(string obj)
        {
            gridMap = FindObjectOfType<Grid>();
            digTileMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTileMap = GameObject.FindWithTag("WaterMap").GetComponent<Tilemap>();
            if (firstLoadDict[SceneName])
            {
                EventHandler.CallGeneratCropEvent();
                firstLoadDict[SceneName] = false;
            }
            RefershMap();
        }

        private void OnSceneNameTransfer(string obj)
        {
            SceneName = obj;
        }

        /// <summary>
        /// 初始化字典内容
        /// </summary>
        /// <param name="mapData">SO文件</param>
        private void InitTileDetailsData(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.TileProperties)
            {
                TileDetails tileDetails = new TileDetails()
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y,
                };
                string key = tileProperty.tileCoordinate.x + "X" + tileProperty.tileCoordinate.y + "Y" + mapData.SceneName;
                if (GetTileDetails(key) != null)
                {
                    tileDetails = GetTileDetails(key);
                }
                switch (tileProperty.gridType)
                {
                    case GridType.CanCanDig:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.CanDropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.CanPlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.NPCObstacle = tileProperty.boolTypeValue;
                        break;
                }
                if (GetTileDetails(key) != null)
                {
                    tileDetailsDict[key] = tileDetails;
                }
                else
                {
                    tileDetailsDict.Add(key, tileDetails);
                }
            }
        }

        /// <summary>
        /// 检查地图信息
        /// </summary>
        /// <param name="key">地图名字</param>
        /// <returns></returns>
        public TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            return null;
        }
    
        public TileDetails GetKeyDict(Vector3Int pos)
        {
            string key = pos.x + "X" + pos.y + "Y" +SceneName;
          
            return GetTileDetails(key);
        }

        private void SetDigGround(TileDetails tileDetails)
        {
            if(digTileMap !=null)
            digTileMap.SetTile(new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0),digRuleTile);
        }
        private void SetWaterGround(TileDetails tileDetails)
        {
            if (waterTileMap != null)
                waterTileMap.SetTile(new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0), waterRuleTile);
        }
       /// <summary>
       /// 更新瓦片数据
       /// </summary>
       /// <param name="tileDetails"></param>
       public void UpdateTiledetailsDect(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "X" + tileDetails.gridY + "Y" + SceneName;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                tileDetailsDict.Add(key, tileDetails);
            }
        }

        private void RefershMap()
        {
            if(digTileMap!=null)
            digTileMap.ClearAllTiles();
            if(waterTileMap!=null)
            waterTileMap.ClearAllTiles() ;
            foreach(var crop in FindObjectsOfType<Crop>())
            {
                Destroy(crop.gameObject);
            }
            DisPlayMapInformation(SceneName);
        }
        
        /// <summary>
        /// 通过数据更新地图显示
        /// </summary>
        /// <param name="sceneName"></param>
        private void DisPlayMapInformation(string sceneName)
        {
           // Debug.Log("RefershMap");
            foreach (var tile in tileDetailsDict)
            {
                var key = tile.Key;
                var details = tile.Value;
                if(key.Contains(sceneName))
                {
                    if(details.daysSinceDug >-1)
                    {
                        SetDigGround(details);
                    }
                    if(details.daysSinceWatered >-1)
                    {
                        SetWaterGround(details);
                    }
                    if(details.seeItemID >-1)
                    {
                       // Debug.Log("SeedItemID!=-1  +" +details.seeItemID );
                        EventHandler.CallPlantSeedEvent(details.seeItemID, details);
                    }
                }
            }
        }

      /// <summary>
      /// 通过鼠标点击坐标获取Crop组件
      /// </summary>
      /// <param name="mousePos"></param>
      /// <returns></returns>
      public Crop GetCropOfVector3(Vector3 mousePos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
            Crop currentCrop = null;
            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Crop>() != null)
                {
                    currentCrop = colliders[i].GetComponent<Crop>();
                }
            }
            return currentCrop;
        }
        /// <summary>
        /// 判断鼠标位置是否有ReapItem
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="itemDetails"></param>
        /// <returns></returns>
        public bool HarvestReapItemInRadious(Vector3 mousePos,ItemDetails itemDetails)
        {
            reapItemList = new List<ReapItem>();
            Collider2D[] colls = new Collider2D[20];
            Physics2D.OverlapCircleNonAlloc(mousePos, itemDetails.itemUseRadios, colls);
            if(colls.Length > 0)
            {
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i] != null)
                    {
                        if (colls[i].GetComponent<ReapItem>())
                        {
                            ReapItem reapItem = colls[i].GetComponent<ReapItem>();
                            reapItemList.Add(reapItem);
                        }
                    }
                }
            }
         
            return reapItemList.Count > 0;
        }

       /// <summary>
       /// 根据场景名称构建网格范围，输出范围和原点
       /// </summary>
       /// <param name="sceneName">场景名称</param>
       /// <param name="gridDimensions">网格范围</param>
       /// <param name="gridOrigin">网格原点</param>
       /// <returns>是否拥有当前场景信息</returns>
       public bool GetGridDimensions(string sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin)
        {
            gridDimensions = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;
            foreach(var mapData in mapDataList)
            {
                if(mapData.SceneName == sceneName)
                {
                    gridDimensions.x = mapData.gridWidth;
                    gridDimensions.y = mapData.gridHeight;

                    gridOrigin.x = mapData.gridX;
                    gridOrigin.y = mapData.gridY;

                    return true;
                }
            }
            return false;
        }
    }
   
}