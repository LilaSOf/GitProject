using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MFarm.AStar
{
    public class Node : IComparable<Node>
    {
        // Start is called before the first frame update
        public Vector2Int gridPosition;//��������
        public int gCost = 0;//����start���ӵľ���
        public int hCost = 0; //����target���ӵľ���
        public int FCost => hCost + gCost;//��ǰ���ӵ�ֵ
        public bool isObstacle = false;//��ǰ�����Ƿ����ϰ���
        public Node parentNode;//���ڵ�

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            //�Ƚ�ѡ����͵�Fֵ
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;
        }
    }
}