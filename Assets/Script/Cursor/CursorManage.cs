using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MFarm.GridMap;
public class CursorManage : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite seed, tool, commity, normal;
    private Sprite currentCursor;
    private Image CursorImage;
    private RectTransform CursorCanvas;
    [Header("�������ת�����")]
    private Camera mainCamera;
    private Grid grid;

    private Vector3 mapPostion;
    private Vector3Int gridPostion;

    private bool enableMouse;

    [SerializeField]private Transform Player_trans;
    private ItemDetails currentItem;
    private void Start()
    {
        CursorCanvas = GameObject.Find("CursorCanvas").GetComponent<RectTransform>();
        CursorImage = CursorCanvas.GetChild(0).GetComponent<Image>();
        mainCamera = Camera.main;
        grid = GameObject.FindObjectOfType<Grid>();
        currentCursor = normal;
        SetCursorImage(normal);
    }
    private void OnEnable()
    {
        EventHandler.ItemSecletEvent += OnItemSelectEvent;
        EventHandler.BeforeFade += OnBeforeFade;
        EventHandler.AfterFade += OnAfterFade;
    }
    private void OnDisable()
    {
        EventHandler.ItemSecletEvent -= OnItemSelectEvent;
        EventHandler.BeforeFade -= OnBeforeFade;
        EventHandler.AfterFade -= OnAfterFade;
    }

    private void OnBeforeFade(string obj)
    {
        enableMouse = false;
    }

    private void OnAfterFade(string obj)
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    /// <summary>
    /// ���ʱ���������ʽ
    /// </summary>
    /// <param name="itemDetails">�����Ʒ������</param>
    /// <param name="isSelect">�����Ʒ�Ƿ��ڱ�ѡ��״̬</param>
    private void OnItemSelectEvent(ItemDetails itemDetails, bool isSelect)
    {
     
        if (!isSelect ) { currentCursor = normal; enableMouse = false; currentItem = null; }
        else
        {
            currentCursor = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => commity,
                ItemType.ChopTool => tool,
                ItemType.CollectTool => tool,
                ItemType.ReapTool => tool,
                ItemType.HoeTool => tool,
                ItemType.WaterTool => tool,
                ItemType.BreakTool => tool,
                _ => normal
            };
            currentItem = itemDetails;
            enableMouse = true;
        }
    }

    private void Update()
    {
        CheckCursorValue();
        InputMouse();
        if (CursorCanvas = null) { return; }
        CursorImage.transform.position = Input.mousePosition;
        if (!IsSelectUI() && enableMouse)
        {
            SetCursorImage(currentCursor);

        }
        else
        {
            SetCursorImage(normal);
        }
    }
    private void InputMouse()
    {
        if(Input.GetMouseButtonDown(0) && enableMouse)
        {
            EventHandler.MouseClickEvent(mapPostion, currentItem);
        }
    }
    #region //���������ʽ
    /// <summary>
    /// �������ͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        CursorImage.sprite = sprite;
        CursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorVild()
    {
        CursorImage.color = new Color(1, 1, 1, 1);
    }
    private void SetCursorInVild()
    {
        CursorImage.color = new Color(1, 0, 0,0.5f);
    }
    #endregion
   /// <summary>
   /// �ж����λ���߼�
   /// </summary>
   private void CheckCursorValue()
    {
        mapPostion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        gridPostion = grid.WorldToCell(mapPostion);
        Vector3Int playerPos = grid.WorldToCell(Player_trans.position);
        TileDetails tileDetails = GridMapManage.Instance.GetKeyDict(gridPostion);
      
        if (Mathf.Abs(gridPostion.x - playerPos.x)>currentItem.itemUseRadios || Mathf.Abs(gridPostion.y - playerPos.y)> currentItem.itemUseRadios)
        {
            SetCursorInVild();
            return;
        }
     //   Debug.Log("�����������");
        if (tileDetails != null)
        {

            switch(currentItem.itemType)
            {
                case ItemType.Commodity:
                    if(currentItem.canDropped && tileDetails.canDropItem) { SetCursorVild(); }
                    break;
            }
        }
        else
        {
            SetCursorInVild();
        }
    }
   /// <summary>
   /// �ж��Ƿ�Ӵ���UI
   /// </summary>
   /// <returns></returns>
   private bool IsSelectUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {return true;}
        else return false;
    }
}
