using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        private SpriteRenderer spriteRenderer;
        public ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        private void Start()
        {
            if(itemID!=0)
            Init(itemID);
        }
        public void Init(int ID)
        {
            itemID = ID;
            itemDetails = InventoryManage.Instance.GetItemDetails(itemID);
            if(itemDetails != null )
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite == null ?itemDetails.itemIcon : itemDetails.itemOnWorldSprite;
                BoxCollider2D coll = GetComponent<BoxCollider2D>();
                //ÐÞ¸ÄÅö×²Ìå³ß´ç
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}

