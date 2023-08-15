using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBarButton : MonoBehaviour
    {
        // Start is called before the first frame update
        public KeyCode key;
        private SlotUI slotUI;
        // Update is called once per frame
        private void Start()
        {
            slotUI = GetComponent<SlotUI>();
        }
        void Update()
        {
            if(Input.GetKeyDown(key)) 
            {
                slotUI.isSelect = !slotUI.isSelect;
                if(slotUI.isSelect )
                {
                    slotUI.inventoryUI.UpdateUISlotHightlight(slotUI.SlotIndex);
                }
                else
                {
                    slotUI.inventoryUI.UpdateUISlotHightlight(-1);
                }
                EventHandler.CallItemSecletEvent(slotUI.itemDetails, slotUI.isSelect);
            }
        }
    }
}