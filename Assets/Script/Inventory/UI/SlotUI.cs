using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("�����ȡ")]
    [SerializeField] private Image slotImage;
    [SerializeField] private Image slotHightLight;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    [Header("��������")]
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
}
