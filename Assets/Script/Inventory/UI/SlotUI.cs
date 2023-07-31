using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using MFarm.Inventory;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
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

    private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
    private void Start()
    {
        isSelect = false;
        if(itemDetails.ID == 0)
        {
            UpdateEmptySlot();
        }
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
        }
        slotImage.enabled = false;
        amountText.text = "";
        button.interactable = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemAmount == 0) return;
        isSelect = true;
        inventoryUI.UpdateUISlotHightlight(SlotIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemAmount != 0)
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
            }
        }
        else
        {
            //�ڵ�ͼ����������
            if(itemDetails.canDropped)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                EventHandler.CallInstantiateItemInScene(itemDetails.ID, pos);
            }
        }
        inventoryUI.UpdateUISlotHightlight(-1);
    }
}
