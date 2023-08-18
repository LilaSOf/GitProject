using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MapData",menuName ="Map/MapData")]
public class MapData_SO :ScriptableObject
{
    // Start is called before the first frame update
    [SceneName]
    public string SceneName;
    [Header("地图信息")]
    public int gridWidth;
    public int gridHeight;

    [Header("左下角原点")]
    public int gridX;
    public int gridY;
    public List<TileProperty> TileProperties;
}
