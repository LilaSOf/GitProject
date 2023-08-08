using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
namespace MFarm.GridMap
{
    public class GridMapManage : Singleton<GridMapManage>
    {
        // Start is called before the first frame update
        public List<MapData_SO> mapDataList = new List<MapData_SO>();
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
        private string SceneName;
        private Grid gridMap;
        private void Start()
        {
            foreach (var mapdataList in mapDataList)
            {
                InitTileDetailsData(mapdataList);
            }
           
        }
        private void OnEnable()
        {
            EventHandler.SceneNameTransfer += OnSceneNameTransfer;
            EventHandler.AfterFade += OnAfterFade;
            EventHandler.ExcuteActionAfterAnimation += OnExcuteActionAfterAnimation;
        }
        private void OnDisable()
        {
            EventHandler.SceneNameTransfer -= OnSceneNameTransfer;
            EventHandler.AfterFade -= OnAfterFade;
            EventHandler.ExcuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
        }


     /// <summary>
     /// 执行实际的逻辑操作
     /// </summary>
     /// <param name="mouseWorldPos">鼠标的世界坐标</param>
     /// <param name="details">物品信息</param>
     private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails details)
        {
            Vector3Int mouseGridPos = gridMap.WorldToCell(mouseWorldPos);
            if (details != null)
            {
                switch (details.itemType)
                {
                    case ItemType.Seed:
                        break;
                }
            }  
        }

        private void OnAfterFade(string obj)
        {
            gridMap = FindObjectOfType<Grid>();
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
        private TileDetails GetTileDetails(string key)
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
            TileDetails tileDetails;
            if(tileDetailsDict.TryGetValue(key, out tileDetails))
            {
                return (tileDetails);
            }
            return null;
        }
    }
}