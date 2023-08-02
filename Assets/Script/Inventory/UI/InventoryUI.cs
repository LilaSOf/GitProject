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
        [Header("人物背包组件")]
        [SerializeField] private GameObject bagUI;
        [Header("拖拽UI")]
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
     /// <param name="location">容器的类型</param>
     /// <param name="list">容器列表</param>
     private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)//更新背包中的UI显示
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
      /// 控制背包的打开和关闭
      /// </summary>
      public void OpenedBag()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }
       /// <summary>
       /// 设置格子高亮
       /// </summary>
       /// <param name="index">高亮格子的序号</param>
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