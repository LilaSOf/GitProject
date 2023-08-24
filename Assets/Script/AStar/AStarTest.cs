using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace MFarm.AStar
{
    public class AStarTest : MonoBehaviour
    {
        // Start is called before the first frame update
        [SceneName]
        public string sceneName;
        public Vector2Int startPos;
        public Vector2Int endPos;
        private AStar aStar;

        public bool isShow;
        public bool isMove;
        public Stack<MovementStep> step;

        public Tilemap tilemap;
        public TileBase tileBase;

        public NPCMovement nPCMovement;
        public bool isMovement;
        public Vector3Int targetPos;
        public AnimationClip clip;
        private void Awake()
        {
            step = new Stack<MovementStep>();
            aStar = GetComponent<AStar>();
        }
        private void Update()
        {
            ShowPathOnGridMap();
            if(isMovement)
            {
                isMovement = false;
                NPCDetails nPCDetails = new NPCDetails(0, 0, 0, Season.´ºÌì, sceneName, targetPos, clip, 0, true);
                nPCMovement.BulidPath(nPCDetails);
                foreach (MovementStep step in step)
                {
                    tilemap.SetTile((Vector3Int)step.gridCoordinate, tileBase);
                }
            }
        }

        private void ShowPathOnGridMap()
        {
            if(tileBase!=null && tilemap !=null)
            {
                if(isShow)
                {
                    tilemap.SetTile((Vector3Int)startPos, tileBase);
                    tilemap.SetTile((Vector3Int)endPos, tileBase);
                }
                else
                {
                    tilemap.SetTile((Vector3Int)startPos, null);
                    tilemap.SetTile((Vector3Int)endPos, null);
                }

                if (isMove)
                {
                    aStar.BuildPath(sceneName, startPos, endPos,step);
                    foreach(MovementStep step in step)
                    {
                        tilemap.SetTile((Vector3Int)step.gridCoordinate, tileBase);
                    }
                }
                else
                {
                   
                }
            }
        }
    }
}
