using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MFarm.Inventory
{
    public class itemPickUp : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                if (item.itemDetails.canPickUp)
                    InventoryManage.Instance.AddItem(item, true);
            }
        }
    }
}

