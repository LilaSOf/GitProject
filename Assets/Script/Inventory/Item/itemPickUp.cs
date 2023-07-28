using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class itemPickUp : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            Item item = other.GetComponent<Item>();
            if(item != null)
            {
                if(item.itemDetails.canPickUp)
                InventoryManage.Instance.AddItem(item, true);
            }
        }
    }
}

