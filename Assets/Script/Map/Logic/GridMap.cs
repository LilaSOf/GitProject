using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    // Start is called before the first frame update
    public MapData_SO mapData;
   [SerializeField] private GridType gridType;
    private Tilemap tileMap;

    private ItemDetails itemDetails;
    private void OnEnable()
    {
        if(!Application.IsPlaying(this))
        {
            tileMap = GetComponent<Tilemap>();
            if(mapData!=null)
            {
                mapData.TileProperties.Clear();
            }
        }
        EventHandler.AfterFade += OnAfterFade;

    }
    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            tileMap = GetComponent<Tilemap>();
            UpdateTileMapProperty();

            if(mapData!=null) { EditorUtility.SetDirty(mapData);}
        }
        EventHandler.AfterFade -= OnAfterFade;
    }

    private void OnAfterFade(string obj)
    {
       
    }

    private void UpdateTileMapProperty()
    {
        tileMap.CompressBounds();
        if(!Application.IsPlaying (this))
        {
            Vector3Int startPoint = tileMap.cellBounds.min;
            Vector3Int endPoint = tileMap.cellBounds.max;
            for(int x= startPoint.x; x<= endPoint.x; x++)
            {
                for(int y= startPoint.y; y<= endPoint.y; y++)
                {
                    TileBase tile = tileMap.GetTile(new Vector3Int(x, y,0));
                    if(tile != null)
                    {
                        TileProperty tileProperty = new TileProperty { tileCoordinate =new Vector2Int(x,y), gridType = this.gridType,boolTypeValue= true};
                        mapData.TileProperties.Add(tileProperty);
                    }
                  
                }
            }
        }
    }
}
