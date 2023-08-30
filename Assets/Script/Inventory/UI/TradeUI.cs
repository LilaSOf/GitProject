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
  /// ����UI�����ʾ
  /// </summary>
  /// <param name="itemDetails">��Ʒ����</param>
  /// <param name="isSell">�Ƿ�����</param>
  public void SetUpUI(ItemDetails itemDetails,bool isSell)
    {
        nameText.text = itemDetails.itemName;
        iconImage.sprite = itemDetails.itemIcon;
        inputField.text = string.Empty;
        this.it = itemDetails;
        isSellTrade = isSell;
    }
    /// <summary>
    /// ȡ���������
    /// </summary>
    public void CancelTrade()
    {
        this.gameObject.SetActive(false);
        inputField.text =string .Empty;
    }
    /// <summary>
    /// ȷ������
    /// </summary>
    public void EnterTrade()
    {
        InventoryManage.Instance.TradeItem(it, tradeAmount,isSellTrade);
        CancelTrade();
    }
   /// <summary>
   /// ����������ť
   /// </summary>
   public void AddBtn()
    {
        tradeAmount++;
        inputField.text = tradeAmount.ToString();
    }
    /// <summary>
    /// ����������ť
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
    /// ��Input File����ֵ����tradeAmount��ȡ��ֵ
    /// </summary>
    public void AmountChange()
    {
        tradeAmount =Convert.ToInt32(inputField.text);
    }
}
