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
    }
}

