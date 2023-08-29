using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryBag_SO Data_SO;
    private bool isOpen;

    private void Update()
    {
         if(isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //πÿ±’√Ê∞Â
            EventHandler.CallBagBaseCloseEvent();
            EventHandler.CallGameStateControllerEvent(GameState.Start);
        }
    }

    public void OnBagOpen()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, Data_SO);
        EventHandler.CallGameStateControllerEvent(GameState.Pasue);
    }
}
