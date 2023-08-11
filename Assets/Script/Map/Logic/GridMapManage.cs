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
        [Header("��ȡ��Ƭ")]
        public RuleTile digRuleTile;
        public RuleTile waterRuleTile;
        private Tilemap digTileMap;
        private Tilemap waterTileMap;

        // Start is called before the first frame update
        [Header("��ȡ��ͼ��Ϣ")]
        public List<MapData_SO> mapDataList = new List<MapData_SO>();
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
        private string SceneName;
        private Grid gridMap;

        [Header("ʱ����µ�ͼ���")]
        private Season currentSeason;
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
            EventHandler.GameDayEvent += OnGameDayEvent;
        }

        private void OnDisable()
        {
            EventHandler.SceneNameTransfer -= OnSceneNameTransfer;
            EventHandler.AfterFade -= OnAfterFade;
            EventHandler.ExcuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }


     /// <summary>
     /// ִ��ʵ�ʵ��߼�����
     /// </summary>
     /// <param name="mouseWorldPos">������������</param>
     /// <param name="details">��Ʒ��Ϣ</param>
     private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails details)
        {
            Vector3Int mouseGridPos = gridMap.WorldToCell(mouseWorldPos);
            var currentTile = GetKeyDict(mouseGridPos);
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
                        //��Ч
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.canDig = false;
                        currentTile.daysSinceWatered = 0;
                        //��Ч
                        break;
                    case ItemType.CollectTool:
                        Crop currentCrop = GetCropOfVector3(mouseWorldPos);
                        if (currentCrop != null) { currentCrop.ProcessToolAction(details); }
                        break;
                }
                UpdateTiledetailsDect(currentTile);
            }  
        }

        /// <summary>
        /// ÿ��ˢ��һ��
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

            RefershMap();
        }

        private void OnSceneNameTransfer(string obj)
        {
            SceneName = obj;
        }

        /// <summary>
        /// ��ʼ���ֵ�����
        /// </summary>
        /// <param name="mapData">SO�ļ�</param>
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
        /// ����ͼ��Ϣ
        /// </summary>
        /// <param name="key">��ͼ����</param>
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
       /// ������Ƭ����
       /// </summary>
       /// <param name="tileDetails"></param>
       private void UpdateTiledetailsDect(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "X" + tileDetails.gridY + "Y" + SceneName;
            tileDetailsDict[key] = tileDetails;
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
        /// ͨ�����ݸ��µ�ͼ��ʾ
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

        private Crop GetCropOfVector3(Vector3 mousePos)
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
    }
   
}