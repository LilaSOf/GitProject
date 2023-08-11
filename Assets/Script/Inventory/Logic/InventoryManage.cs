using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManage : Singleton<InventoryManage>
    {
        // Start is called before the first frame update
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        [Header("背包数据")]
        public InventoryBag_SO PlayerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvenet;
            EventHandler.PlantSeedEvent += OnPlaneSeedEvent;
            EventHandler.HarvestInPlayerPostion += OnHarvestInPlayerPostion;
        }
        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvenet;
            EventHandler.PlantSeedEvent -= OnPlaneSeedEvent;
            EventHandler.HarvestInPlayerPostion -= OnHarvestInPlayerPostion;
        }

        private void OnHarvestInPlayerPostion(int itemID)
        {
            int index = GetItemIndexInBag(itemID);
            AddItemInIndex(itemID, 1, index);

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }

        private void OnPlaneSeedEvent(int ID, TileDetails details)
        {
           // RemoveItem(ID, 1);
        }

        private void OnDropItemEvenet(int ID, Vector3 Pos)
        {
            RemoveItem(ID, 1);
        }

        /// <summary>
        /// 通过ID返回物品信息
        /// </summary>
        /// <param name="ID">itemID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.ItemDataList.Find(i  => i.ID == ID); 
        }

      /// <summary>
      /// 向背包当中添加物品
      /// </summary>
      /// <param name="item">需要添加的物品</param>
      /// <param name="IsDestroy">是否需要销毁该物品</param>
      public void AddItem(Item item,bool IsDestroy)
        {
            if(IsDestroy)
            {
                Destroy(item.gameObject);
            }
            var index = GetItemIndexInBag(item.itemID);
            AddItemInIndex(item.itemID,1,index);

            //更新背包UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
       /// <summary>
       /// 检查背包是否有空位
       /// </summary>
       /// <returns></returns>
       private bool CheakBagCapacity()
        {
            for(int i = 0;i < PlayerBag.itemList.Count;i++)
            {
                if (PlayerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
       /// <summary>
       /// 查找相同物品的序列号
       /// </summary>
       /// <param name="ID">传入物品的ID</param>
       /// <returns>-1则没有该物品返回序号</returns>
       private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if(PlayerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 通过背包序列号向背包中添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="amount">物品数量</param>
        /// <param name="index">物品序号</param>
        private void AddItemInIndex(int ID,int amount,int index)
        {
            if(index == -1 && CheakBagCapacity())//背包当中没有该物品 同时背包有空位
            {
                InventoryItem it =new InventoryItem() { itemID =ID,itemAmount=amount};
                for (int i = 0; i < PlayerBag.itemList.Count; i++)
                {
                    if (PlayerBag.itemList[i].itemID == 0)
                    {
                        PlayerBag.itemList[i] = it;
                        break;
                    }
                }
            }
            else//背包有该物品进行数量叠加
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount + amount;
                InventoryItem it = new InventoryItem() { itemID = ID, itemAmount = currentAmount };
                PlayerBag.itemList[index] = it;
            }
        }

       /// <summary>
       /// 背包之间的物品数据交换
       /// </summary>
       /// <param name="formIndex">拖拽序列号</param>
       /// <param name="targetIndex">目标序列号</param>
       public void SwapItem(int formIndex,int targetIndex)
        {
            var currentItem = PlayerBag.itemList[formIndex];
            var targetItem = PlayerBag.itemList[targetIndex];

            if(targetItem.itemID != 0)
            {
                PlayerBag.itemList[formIndex] = targetItem;
                PlayerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                PlayerBag.itemList[targetIndex] = currentItem;
                PlayerBag.itemList[formIndex] = new InventoryItem();
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
        /// <summary>
        /// 移除背包当中的物品
        /// </summary>
        /// <param name="ItemID">物品ID</param>
        /// <param name="removeAmount">物品数量</param>
        public void RemoveItem(int ItemID,int removeAmount)
        {
            var index = GetItemIndexInBag(ItemID);
            if (PlayerBag.itemList[index].itemAmount>removeAmount)
            {
                int amount = PlayerBag.itemList[index].itemAmount - removeAmount; 
                var item = new InventoryItem() {itemID = ItemID,itemAmount =amount };
                PlayerBag.itemList[index] = item;
            }
            else if(PlayerBag.itemList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                PlayerBag.itemList[index] = item;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,PlayerBag.itemList);
        }
    }


}

