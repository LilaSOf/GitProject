using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using MFarm.Inventory;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    // Start is called before the first frame update
    [Header("组件获取")]
    [SerializeField] private Image slotImage;
    public Image slotHightLight;//高亮部分
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    [Header("格子类型")]
    public SlotType slotType;

    public ItemDetails itemDetails;
    public int itemAmount;
    public bool isSelect;//是否被选中

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
    /// 更新格子和UI信息
    /// </summary>
    /// <param name="it">数据</param>
    /// <param name="amount">持有数量</param>
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
   /// 更新格子为空的状态
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
        isSelect = !isSelect;
        inventoryUI.UpdateUISlotHightlight(SlotIndex);

        if(slotType == SlotType.Bag)
        {
            EventHandler.CallItemSecletEvent(itemDetails, isSelect);
        }
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

    public void OnEndDrag(PointerEventData eventData)//拖拽结束
    {
        inventoryUI.DragItemImage.enabled = false;
        if((eventData.pointerCurrentRaycast.gameObject != null))
        {
            var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
            if (targetSlot != null)
            {
                int targetIndex = targetSlot.SlotIndex;
                //背包当中的数据传递
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManage.Instance.SwapItem(SlotIndex, targetIndex);
                }
            }
        }
        else
        {
            //在地图上生成物体
            if(itemDetails.canDropped)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                EventHandler.CallInstantiateItemInScene(itemDetails.ID, pos);
            }
        }
        inventoryUI.UpdateUISlotHightlight(-1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemAmount != 0)
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
