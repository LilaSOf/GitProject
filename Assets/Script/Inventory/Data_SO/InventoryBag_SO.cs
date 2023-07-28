using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/BagData")]
public class InventoryBag_SO : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
   public List<InventoryItem> itemList = new List<InventoryItem>();
}
