using MFarm.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.GridMap;
using Unity.VisualScripting;
using Unity.Mathematics;

namespace MFarm.AStar
{
    public class AStar : MonoBehaviour
    {
        // Start is called before the first frame update
        public GridNodes gridNodes;
        public Node startNode;
        public Node targetNode;
        [Header("地图大小")]
        public int gridWidth;
        public int gridHeight;
        [Header("地图原点x,y坐标")]
        private int originX;
        private int originY;
        private List<Node> openNodeList;//当前选中node周围的8个点

        private HashSet<Node> closeNodeList;//所有被选中的点 

        private bool pathFind;


        /// <summary>
        /// 构建路径
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="startPos">起点坐标</param>
        /// <param name="endPos">终点坐标</param>
        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos,Stack<MovementStep> movementSteps)
        {
            pathFind = false;

            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //查找最短路径
                if (FindShortsPath())
                {
                    //构建NPC移动路径
                    UpdatePathOnMovementStepStack(sceneName, movementSteps);
                }
            }
        }



        /// <summary>
        /// 构建网格节点信息，初始化两个列表
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        /// <param name="startPos">起点</param>
        /// <param name="endPos">终点</param>
        /// <returns></returns>
        public bool GenerateGridNodes(string sceneName, Vector2Int startPos, Vector2Int endPos)
        {
            if (GridMapManage.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin))
            {
                //根据地图瓦片信息生成网络移动节点范围数组
                gridNodes = new GridNodes(gridDimensions.x, gridDimensions.y);
                gridHeight = gridDimensions.y;
                gridWidth = gridDimensions.x;
                originX = gridOrigin.x;
                originY = gridOrigin.y;

                openNodeList = new List<Node>();
                closeNodeList = new HashSet<Node>();
            }
            else
                return false;
            //grid节点从0，0开始，减去原点坐标获得实际位置
            startNode = gridNodes.GetGridNode(startPos.x - originX, startPos.y - originY);
            targetNode = gridNodes.GetGridNode(endPos.x - originX, endPos.y - originY);

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    int newX = x + originX;
                    int newY = y + originY;
                    var key = newX + "X" +newY + "Y" + sceneName;
                    TileDetails tileDetails = GridMapManage.Instance.GetTileDetails(key);

                    if (tileDetails != null)
                    {
                     
                        Node node = gridNodes.GetGridNode(x, y);
                        if (tileDetails.NPCObstacle)
                            node.isObstacle = true;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 寻找最短路径
        /// </summary>
        /// <returns></returns>
        private bool FindShortsPath()
        {
            //添加起点
            openNodeList.Add(startNode);
            while (openNodeList.Count > 0)
            {
                //节点排序，node内涵比较函数
                openNodeList.Sort();

                Node closeNode = openNodeList[0];
                openNodeList.RemoveAt(0);

                closeNodeList.Add(closeNode);

                if (closeNode == targetNode)
                {
                    pathFind = true;
                    break;
                }

                //计算周围8个节点的情况，添加进openNodeList
                EvaluateNeighbourNodes(closeNode);
            }
            return pathFind;
        }
       /// <summary>
       /// 评估周围8个点并得到对应的消耗值
       /// </summary>
       /// <param name="currentNode"></param>
       private void EvaluateNeighbourNodes(Node currentNode)
        {
            Vector2Int currentPos = currentNode.gridPosition;
            Node neighbourNode;
            for(int x =-1; x <=1; x++)
            {
                for(int y = -1; y <=1; y++)
                {
                    if(x==0&&y==0)
                    {
                        continue;
                    }
                    neighbourNode = GetValidNeighbourNode(currentPos.x+x, currentPos.y+y);//获取周围八个节点
                    if(neighbourNode != null)
                    {
                        if(!openNodeList.Contains(neighbourNode))
                        {
                            //计算gCost和gCost
                            neighbourNode.gCost = currentNode.gCost + GetDistance(currentNode,neighbourNode);
                            neighbourNode.hCost = GetDistance(neighbourNode, targetNode);
                            neighbourNode.parentNode = currentNode;
                            openNodeList.Add(neighbourNode);
                        }
                    }
                }
            }
        }
       /// <summary>
       /// 找到并返回有效的Node（非障碍，非边界外）
       /// </summary>
       /// <param name="x"></param>
       /// <param name="y"></param>
       /// <returns></returns>
       private Node GetValidNeighbourNode(int x, int y)
        {
            if(x > gridWidth || y > gridHeight || x<0||y<0)
            { return null; }
            Node neighbourNode  = gridNodes.GetGridNode(x,y);

            if (neighbourNode.isObstacle || closeNodeList.Contains(neighbourNode))
                return null;
            else
            {
                return neighbourNode;
            }
        }

      /// <summary>
      /// 返回任意两点之间的距离值
      /// </summary>
      /// <param name="nodeA"></param>
      /// <param name="nodeB"></param>
      /// <returns></returns>
      private int GetDistance(Node nodeA, Node nodeB)
        {
            int Xdistance = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
            int Ydistance = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

            if(Xdistance > Ydistance)
            {
                return 14*Ydistance + 10*(Xdistance-Ydistance);
            }
            return 14 * Xdistance + 10 * (Ydistance - Xdistance);
        }

        private void UpdatePathOnMovementStepStack(string sceneName,Stack<MovementStep> movementSteps)
        {
            Node nextNode = targetNode;
            while (nextNode != null)
            {
                MovementStep newStep = new MovementStep();

                newStep.sceneName = sceneName;
                newStep.gridCoordinate = new Vector2Int(nextNode.gridPosition.x +originX,nextNode.gridPosition.y +originY);
                movementSteps.Push(newStep);
                //压入堆栈
                nextNode = nextNode.parentNode;
            }
        }
    }
}