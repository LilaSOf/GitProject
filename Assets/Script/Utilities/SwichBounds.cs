using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichBounds : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void Start()
    {
        SwitchConfinerShape();
    }
    //切换场景后调用
    private void SwitchConfinerShape()
    {
        PolygonCollider2D  ConfinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = ConfinerShape;

        confiner.InvalidatePathCache();//每次更换边界时需要调用此函数清理缓存
    }
}
