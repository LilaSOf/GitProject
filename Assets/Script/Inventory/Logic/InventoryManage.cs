using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManage : Singleton<InventoryManage>
    {
        // Start is called before the first frame update
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;
        [Header("��������")]
        public InventoryBag_SO PlayerBag;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvenet;
        }
        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvenet;
        }

        private void OnDropItemEvenet(int ID, Vector3 Pos)
        {
            RemoveItem(ID, 1);
        }

        /// <summary>
        /// ͨ��ID������Ʒ��Ϣ
        /// </summary>
        /// <param name="ID">itemID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.ItemDataList.Find(i  => i.ID == ID); 
        }

      /// <summary>
      /// �򱳰����������Ʒ
      /// </summary>
      /// <param name="item">��Ҫ��ӵ���Ʒ</param>
      /// <param name="IsDestroy">�Ƿ���Ҫ���ٸ���Ʒ</param>
      public void AddItem(Item item,bool IsDestroy)
        {
            if(IsDestroy)
            {
                Destroy(item.gameObject);
            }
            var index = GetItemIndexInBag(item.itemID);
            AddItemInIndex(item.itemID,1,index);

            //���±���UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }
       /// <summary>
       /// ��鱳���Ƿ��п�λ
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
       /// ������ͬ��Ʒ�����к�
       /// </summary>
       /// <param name="ID">������Ʒ��ID</param>
       /// <returns>-1��û�и���Ʒ�������</returns>
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
        /// ͨ���������к��򱳰��������Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="amount">��Ʒ����</param>
        /// <param name="index">��Ʒ���</param>
        private void AddItemInIndex(int ID,int amount,int index)
        {
            if(index == -1 && CheakBagCapacity())//��������û�и���Ʒ ͬʱ�����п�λ
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
            else//�����и���Ʒ������������
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount + amount;
                InventoryItem it = new InventoryItem() { itemID = ID, itemAmount = currentAmount };
                PlayerBag.itemList[index] = it;
            }
        }

       /// <summary>
       /// ����֮�����Ʒ���ݽ���
       /// </summary>
       /// <param name="formIndex">��ק���к�</param>
       /// <param name="targetIndex">Ŀ�����к�</param>
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
        /// �Ƴ��������е���Ʒ
        /// </summary>
        /// <param name="ItemID">��ƷID</param>
        /// <param name="removeAmount">��Ʒ����</param>
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

