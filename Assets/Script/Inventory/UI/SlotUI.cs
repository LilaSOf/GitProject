using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using MFarm.Inventory;
using UnityEditor;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    // Start is called before the first frame update
    [Header("�����ȡ")]
    [SerializeField] private Image slotImage;
    public Image slotHightLight;//��������
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    [Header("��������")]
    public SlotType slotType;

    public ItemDetails itemDetails;
    public int itemAmount;
    public bool isSelect;//�Ƿ�ѡ��

    public int SlotIndex;

    [SerializeField]private bool MouseIntercable;

    public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
    private void Start()
    {
        isSelect = false;
        if(itemDetails == null )
        {
            UpdateEmptySlot();
        }
    
    }
    private void Update()
    {
       
    }
    private void OnEnable()
    {
        EventHandler.DropItemInBagEvent += OnDropItemInBagEvent;
    }
    private void OnDisable()
    {
        EventHandler.DropItemInBagEvent -= OnDropItemInBagEvent;
    }

    private void OnDropItemInBagEvent(bool mouseIntercable)
    {
        MouseIntercable = mouseIntercable;
    }

    /// <summary>
    /// ���¸��Ӻ�UI��Ϣ
    /// </summary>
    /// <param name="it">����</param>
    /// <param name="amount">��������</param>
    public void UpdateSlot(ItemDetails it,int amount)
    {
        slotImage.sprite = it.itemIcon;
        itemDetails = it;
        itemAmount = amount;
        amountText.text = amount.ToString();
        slotImage.enabled = true;
        button.interactable = true;
    }

   /// <summary>
   /// ���¸���Ϊ�յ�״̬
   /// </summary>
   public void UpdateEmptySlot()
    {
        if(isSelect)
        {
            isSelect = false;
            inventoryUI.UpdateUISlotHightlight(-1);
            EventHandler.CallItemSecletEvent(itemDetails, isSelect);
        }
        itemDetails = null;
        slotImage.enabled = false;
        amountText.text = "";
        button.interactable = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemDetails == null) return;
        if(itemDetails != null)
        {
            isSelect = !isSelect;
            inventoryUI.UpdateUISlotHightlight(SlotIndex);

            if (slotType == SlotType.Bag)
            {
                EventHandler.CallItemSecletEvent(itemDetails, isSelect);
            }
            EventHandler.CallItemSelectEvent(itemDetails, isSelect);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            inventoryUI.DragItemImage.enabled = true;
            inventoryUI.DragItemImage.sprite = slotImage.sprite;
            inventoryUI.DragItemImage.SetNativeSize();

            isSelect = true;
            inventoryUI.UpdateUISlotHightlight(SlotIndex);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventoryUI.DragItemImage.gameObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)//��ק����
    {
        inventoryUI.DragItemImage.enabled = false;
        if((eventData.pointerCurrentRaycast.gameObject != null))
        {
            var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
            if (targetSlot != null)
            {
                int targetIndex = targetSlot.SlotIndex;
                //�������е����ݴ���
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManage.Instance.SwapItem(SlotIndex, targetIndex);
                }
                if(slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop)//������Ʒ
                {
                    EventHandler.CallTradeEvent(itemDetails, true);
                }
                if(slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)//������Ʒ
                {
                    EventHandler.CallTradeEvent(itemDetails, false);
                }
            }
        }
        else
        {
            if (!MouseIntercable) return;
            //�ڵ�ͼ����������
            else
            {
                if(itemDetails!= null)
                {
                    if (itemDetails.canDropped)
                        EventHandler.CallDropItemEvent(itemDetails.ID, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
                  // EventHandler.CallUpdateInventoryUI
                    //EventHandler.CallItemSecletEvent(itemDetails, isSelect);
                }              
            }
        }
        inventoryUI.UpdateUISlotHightlight(-1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(true);
            inventoryUI.itemToolTip.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
            inventoryUI.itemToolTip.SetupToolTip(itemDetails, slotType);
        }
        else
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.itemToolTip.gameObject.SetActive(false);
    }
}
