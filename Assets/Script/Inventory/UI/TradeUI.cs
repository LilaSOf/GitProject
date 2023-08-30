using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MFarm.Inventory;
using System;
public class TradeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text nameText;
    public Image iconImage;
    public Button enterBtn;
    public Button exitBtn;
    public InputField inputField;
    private bool isSellTrade;
    private ItemDetails it;
    [SerializeField]private int tradeAmount = 0;
  /// <summary>
  /// 设置UI面板显示
  /// </summary>
  /// <param name="itemDetails">物品数据</param>
  /// <param name="isSell">是否销售</param>
  public void SetUpUI(ItemDetails itemDetails,bool isSell)
    {
        nameText.text = itemDetails.itemName;
        iconImage.sprite = itemDetails.itemIcon;
        inputField.text = string.Empty;
        this.it = itemDetails;
        isSellTrade = isSell;
    }
    /// <summary>
    /// 取消交易面板
    /// </summary>
    public void CancelTrade()
    {
        this.gameObject.SetActive(false);
        inputField.text =string .Empty;
    }
    /// <summary>
    /// 确定购买
    /// </summary>
    public void EnterTrade()
    {
        InventoryManage.Instance.TradeItem(it, tradeAmount,isSellTrade);
        CancelTrade();
    }
   /// <summary>
   /// 增加数量按钮
   /// </summary>
   public void AddBtn()
    {
        tradeAmount++;
        inputField.text = tradeAmount.ToString();
    }
    /// <summary>
    /// 减少数量按钮
    /// </summary>
    public void ReduceBtn()
    {
        if(tradeAmount > 0)
        {
            tradeAmount--;
            inputField.text = tradeAmount.ToString();
        }
    }
    /// <summary>
    /// 在Input File输入值后，让tradeAmount获取该值
    /// </summary>
    public void AmountChange()
    {
        tradeAmount =Convert.ToInt32(inputField.text);
    }
}
