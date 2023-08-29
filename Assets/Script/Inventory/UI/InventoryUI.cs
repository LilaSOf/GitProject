using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        [Header("ͨ�ñ���")]
        [SerializeField] private GameObject baseBag;
        public GameObject shopPrefab;

        [SerializeField] private List<SlotUI> baseBagList;
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
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            EventHandler.BagBaseCloseEvent += OnBagBaseCloseEvent;
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
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            EventHandler.BagBaseCloseEvent -= OnBagBaseCloseEvent;
        }

       /// <summary>
       /// �رձ������߼��¼�
       /// </summary>
       private void OnBagBaseCloseEvent()
        {
            baseBag.SetActive(false);
            itemToolTip.gameObject.SetActive(false);
            UpdateUISlotHightlight(-1);
            bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            bagUI.SetActive(false);
            bagOpened = false;
            foreach (SlotUI slotUI in baseBagList)
            {
                Destroy(slotUI.gameObject);
            }
            baseBagList.Clear();
        }

        /// <summary>
        /// �򿪱������߼��¼�
        /// </summary>
        /// <param name="slotType">��ǰ���ӵ�����</param>
        /// <param name="data_SO">��ǰ����������</param>
        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO data_SO)
        {
            GameObject prefab = slotType switch
            {
                SlotType.Shop => shopPrefab,
                _ => null
            };
            baseBag.SetActive(true);
            baseBagList = new List<SlotUI>();
            for(int i = 0;i<data_SO.itemList.Count;i++)
            {
                var slot = Instantiate(prefab,baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
                slot.SlotIndex = i;
                baseBagList.Add(slot);
            }
            if(slotType == SlotType.Shop)
            {
                bagUI.GetComponent<RectTransform>().pivot = new Vector2(-1, 0.5f);
                bagUI.SetActive(true);
                bagOpened = true;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());
            //����UI��ʾ
            OnUpdateInventoryUI(InventoryLocation.Shop, data_SO.itemList);
        }


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
                case InventoryLocation.Shop:
                case InventoryLocation.Box:
                    for (int i = 0; i < baseBagList.Count; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManage.Instance.GetItemDetails(list[i].itemID);
                            baseBagList[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            baseBagList[i].UpdateEmptySlot();
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