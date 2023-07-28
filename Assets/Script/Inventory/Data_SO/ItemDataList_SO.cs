using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemDataList_SO",menuName ="Inventory/ItemDataList")]
public class ItemDataList_SO : ScriptableObject
{
    // Start is called before the first frame update
    public List<ItemDetails> ItemDataList = new List<ItemDetails>();
}
