using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Type;
    [SerializeField] private Text Value;
    [SerializeField] private GameObject ButtomPart;

    public void SetupToolTip(ItemDetails itemDetails,SlotType slotType)
    {
        Name.text = itemDetails.itemName;
        Description.text = itemDetails.Description;
        Type.text = GetItemType(itemDetails.itemType);

        if(itemDetails.itemType == ItemType.Seed||itemDetails.itemType == ItemType.Furniture || itemDetails.itemType == ItemType.Commodity)
        {
            ButtomPart.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag || slotType == SlotType.Box)
            {
                price = (int)(itemDetails.sellPercentage * itemDetails.itemPrice);
            }
            Value.text = price.ToString();
        }
        else
        {
            ButtomPart.SetActive(false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    private string GetItemType(ItemType type)
    {
        return type switch
        {
            ItemType.Furniture => "�Ҿ�",
            ItemType.Commodity => "��Ʒ",
            ItemType.CollectTool => "����",
            ItemType.ReapTool => "����",
            ItemType.HoeTool => "����",
            ItemType.Seed =>"����",
            ItemType.BreakTool => "����",
            _  =>"��"
        };
    }
}
