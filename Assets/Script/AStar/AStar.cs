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
        [Header("��ͼ��С")]
        public int gridWidth;
        public int gridHeight;
        [Header("��ͼԭ��x,y����")]
        private int originX;
        private int originY;
        private List<Node> openNodeList;//��ǰѡ��node��Χ��8����

        private HashSet<Node> closeNodeList;//���б�ѡ�еĵ� 

        private bool pathFind;


        /// <summary>
        /// ����·��
        /// </summary>
        /// <param name="sceneName">��������</param>
        /// <param name="startPos">�������</param>
        /// <param name="endPos">�յ�����</param>
        public void BuildPath(string sceneName, Vector2Int startPos, Vector2Int endPos,Stack<MovementStep> movementSteps)
        {
            pathFind = false;

            if (GenerateGridNodes(sceneName, startPos, endPos))
            {
                //�������·��
                if (FindShortsPath())
                {
                    //����NPC�ƶ�·��
                    UpdatePathOnMovementStepStack(sceneName, movementSteps);
                }
            }
        }



        /// <summary>
        /// ��������ڵ���Ϣ����ʼ�������б�
        /// </summary>
        /// <param name="sceneName">��������</param>
        /// <param name="startPos">���</param>
        /// <param name="endPos">�յ�</param>
        /// <returns></returns>
        public bool GenerateGridNodes(string sceneName, Vector2Int startPos, Vector2Int endPos)
        {
            if (GridMapManage.Instance.GetGridDimensions(sceneName, out Vector2Int gridDimensions, out Vector2Int gridOrigin))
            {
                //���ݵ�ͼ��Ƭ��Ϣ���������ƶ��ڵ㷶Χ����
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
            //grid�ڵ��0��0��ʼ����ȥԭ��������ʵ��λ��
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
        /// Ѱ�����·��
        /// </summary>
        /// <returns></returns>
        private bool FindShortsPath()
        {
            //������
            openNodeList.Add(startNode);
            while (openNodeList.Count > 0)
            {
                //�ڵ�����node�ں��ȽϺ���
                openNodeList.Sort();

                Node closeNode = openNodeList[0];
                openNodeList.RemoveAt(0);

                closeNodeList.Add(closeNode);

                if (closeNode == targetNode)
                {
                    pathFind = true;
                    break;
                }

                //������Χ8���ڵ���������ӽ�openNodeList
                EvaluateNeighbourNodes(closeNode);
            }
            return pathFind;
        }
       /// <summary>
       /// ������Χ8���㲢�õ���Ӧ������ֵ
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
                    neighbourNode = GetValidNeighbourNode(currentPos.x+x, currentPos.y+y);//��ȡ��Χ�˸��ڵ�
                    if(neighbourNode != null)
                    {
                        if(!openNodeList.Contains(neighbourNode))
                        {
                            //����gCost��gCost
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
       /// �ҵ���������Ч��Node�����ϰ����Ǳ߽��⣩
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
      /// ������������֮��ľ���ֵ
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
                //ѹ���ջ
                nextNode = nextNode.parentNode;
            }
        }
    }
}