using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryBag_SO Data_SO;
    private bool isOpen;

    public Button escBtn;//=> GameObject.Find("ESC Image").GetComponent<Button>();

    private void Start()
    {
        
    }
    private void Update()
    {
         if(isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //πÿ±’√Ê∞Â
            OnBagClose();
        }
    }

    public void OnBagOpen()
    {
        isOpen = true;
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, Data_SO);
        EventHandler.CallGameStateControllerEvent(GameState.Pasue);
        escBtn = GameObject.FindGameObjectWithTag("escBtn").GetComponent<Button>();
        escBtn.onClick.AddListener(OnBagClose);
    }
    public void OnBagClose()
    {
        isOpen = false;
        EventHandler.CallBagBaseCloseEvent();
        EventHandler.CallGameStateControllerEvent(GameState.Start);
    }
}
