using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CursorManage : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite seed, tool, commity, normal;
    private Sprite currentCursor;
    private Image CursorImage;
    private RectTransform CursorCanvas;
    private void Start()
    {
        CursorCanvas = GameObject.Find("CursorCanvas").GetComponent<RectTransform>();
        CursorImage = CursorCanvas.GetChild(0).GetComponent<Image>();
        SetCursorImage(normal);
    }
    private void Update()
    {
        if (CursorCanvas = null) { return; }
        CursorImage.transform.position = Input.mousePosition;
    }
    private void SetCursorImage(Sprite sprite)
    {
        CursorImage.sprite = sprite;
    }

}
