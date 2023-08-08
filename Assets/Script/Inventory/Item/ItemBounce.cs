using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        // Start is called before the first frame update
        private Transform spriteRendererTrans;
        private BoxCollider2D coll;
        private Vector3 targetPos;
        private Vector2 dir;
        public float grivaty;
        private float Distance;

        private bool IsGround;

        private void Awake()
        {
            spriteRendererTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false;
        }

        /// <summary>
        /// 初始化物品
        /// </summary>
        /// <param name="target">目标（鼠标）位置</param>
        /// <param name="dirs">方向</param>
        public void InitBounceItem(Vector3 target, Vector2 dirs)
        {
           // coll.enabled = false;
            targetPos = target;
            dir = dirs;
            Distance = Vector3.Distance(targetPos, transform.position);
            spriteRendererTrans.position += Vector3.up * 1.5f;
        }

        private void Update()
        {
            Bounds();
        }
      /// <summary>
      /// 影子和图片的移动
      /// </summary>
      private void Bounds()
        {
            IsGround = spriteRendererTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.2f)
            {
                transform.position += (Vector3)dir * -grivaty * Distance * Time.deltaTime;
            }
            if (!IsGround)
            {
                spriteRendererTrans.position += Vector3.up * -grivaty * Time.deltaTime;
            }
            else
            {
                spriteRendererTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}