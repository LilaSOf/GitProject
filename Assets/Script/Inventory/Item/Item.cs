using MFarm.CropM;
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
        private BoxCollider2D coll;
        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            if (itemID!=0)
            {
                Init(itemID);
            }
        }
        public void Init(int ID)
        {
            itemID = ID;
            itemDetails = InventoryManage.Instance.GetItemDetails(itemID);
            if(itemDetails != null )
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite == null ?itemDetails.itemIcon : itemDetails.itemOnWorldSprite;
                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
            if(itemDetails.itemType == ItemType.ReapableScenery)
            {
                //添加组件
                gameObject.AddComponent<ReapItem>();
                gameObject.GetComponent<ReapItem>().InitCrop(itemDetails.ID);
            }
        }
    }
}

