using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MFarm.AStar
{
    public class Node : IComparable<Node>
    {
        // Start is called before the first frame update
        public Vector2Int gridPosition;//网格坐标
        public int gCost = 0;//距离start格子的距离
        public int hCost = 0; //距离target格子的距离
        public int FCost => hCost + gCost;//当前格子的值
        public bool isObstacle = false;//当前格子是否有障碍物
        public Node parentNode;//父节点

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            //比较选出最低的F值
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;
        }
    }
}