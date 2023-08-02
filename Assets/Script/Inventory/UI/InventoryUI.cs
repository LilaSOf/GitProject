using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private SlotUI[] playerBag;
        [Header("���ﱳ�����")]
        [SerializeField] private GameObject bagUI;
        [Header("��קUI")]
        public Image DragItemImage;
        private bool bagOpened;

        public ItemToolTip itemToolTip;
        private void Start()
        {
            for(int i = 0; i < playerBag.Length; i++)
            {
                playerBag[i].SlotIndex = i; 
            }
            bagOpened = bagUI.activeInHierarchy;
        }
        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                OpenedBag();
            }
        }
        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="location">����������</param>
     /// <param name="list">�����б�</param>
     private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)//���±����е�UI��ʾ
        {
            switch(location)
            {
                case InventoryLocation.Player:
                    for(int i = 0;i<playerBag.Length;i++)
                    {
                        if (list[i].itemAmount >0)
                        {
                            var item = InventoryManage.Instance.GetItemDetails(list[i].itemID);
                            playerBag[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerBag[i].UpdateEmptySlot();
                        }
                    }

                    break;
            }
        }
      /// <summary>
      /// ���Ʊ����Ĵ򿪺͹ر�
      /// </summary>
      public void OpenedBag()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
       /// <summary>
       /// ���ø��Ӹ���
       /// </summary>
       /// <param name="index">�������ӵ����</param>
       public void UpdateUISlotHightlight(int index)
        {
            foreach(var slot in playerBag)
            {
                if(slot.SlotIndex == index && slot.isSelect)
                {
                    slot.slotHightLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelect = false;
                    slot.slotHightLight.gameObject.SetActive(false);
                }
            }
        }
    }
}