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
    private void OnEnable()
    {
        EventHandler.AfterFade += OnAfterFade;
    }
    private void OnDisable()
    {
        EventHandler.AfterFade -= OnAfterFade;
    }

    private void OnAfterFade(string obj)
    {
        SwitchConfinerShape();
    }

    //�л����������
    private void SwitchConfinerShape()
    {
        PolygonCollider2D  ConfinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = ConfinerShape;

        confiner.InvalidatePathCache();//ÿ�θ����߽�ʱ��Ҫ���ô˺���������
    }
}
