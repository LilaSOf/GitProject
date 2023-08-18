using MFarm.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFarm.GridMap;
using Unity.VisualScripting;

public class AStar : MonoBehaviour
{
    // Start is called before the first frame update
   public GridNodes gridNodes;
    public Node startNode;
    public Node targetNode;
    public int gridWidth;
    public int gridHeight;
    private int originX;
    private int originY;
    private List<Node> openNodeList;//��ǰѡ��node��Χ��8����

    private HashSet<Node> closeNodeList;//���б�ѡ�еĵ� 

    private bool pathFind;


    public void BuildPath(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        pathFind = false;

        if(GenerateGridNodes(sceneName, startPos, endPos))
        {
            //�������·��
            if (FindShortsPath())
            {
                //����NPC�ƶ�·��
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
   public bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        if(GridMapManage.Instance.GetGridDimensions(sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin))
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

        for(int x = 0; x < gridWidth;x++)
        {
            for(int y = 0;y <gridHeight;y++)
            {
                var key = x + originX + "X" + y + originY + "Y" + sceneName;
                TileDetails tileDetails = GridMapManage.Instance.GetTileDetails(key);

                if(tileDetails != null)
                {
                    Node node = gridNodes.GetGridNode(x, y);
                    if(tileDetails.NPCObstacle)
                        node.isObstacle = true;
                }
            }
        }

        return true;
    }


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
        }
        return pathFind;
    }

}
