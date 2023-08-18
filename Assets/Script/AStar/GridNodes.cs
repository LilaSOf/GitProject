using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.AStar
{
    public class GridNodes
    {
        // Start is called before the first frame update
        public int height;
        public int width;

        private Node[,] gridNodes;

        public GridNodes(int width,int height)
        {
            this.width = width;
            this.height = height;

            gridNodes = new Node[width,height];
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    gridNodes[x,y] = new Node(new Vector2Int(x,y));
                }
            }
        }
        public Node GetGridNode(int x,int y)
        {
           if(x<width && y<height)
            {
                return gridNodes[x,y];
            }
            Debug.Log("³¬³öÍø¸ñ·¶Î§");
           return null;
        }
    }
}