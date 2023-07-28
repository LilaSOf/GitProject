using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("组件获取")]
    [SerializeField] private Image slotImage;
    [SerializeField] private Image slotHightLight;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    [Header("格子类型")]
    public SlotType slotType;

    public ItemDetails itemDetails;
    public int itemAmount;
    private bool isSelect;
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
}
