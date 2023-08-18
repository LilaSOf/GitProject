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
    private List<Node> openNodeList;//当前选中node周围的8个点

    private HashSet<Node> closeNodeList;//所有被选中的点 

    private bool pathFind;


    public void BuildPath(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        pathFind = false;

        if(GenerateGridNodes(sceneName, startPos, endPos))
        {
            //查找最短路径
            if (FindShortsPath())
            {
                //构建NPC移动路径
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
   public bool GenerateGridNodes(string sceneName,Vector2Int startPos,Vector2Int endPos)
    {
        if(GridMapManage.Instance.GetGridDimensions(sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin))
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
        }
        return pathFind;
    }

}
