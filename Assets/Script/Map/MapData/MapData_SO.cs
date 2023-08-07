using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MapData",menuName ="Map/MapData")]
public class MapData_SO :ScriptableObject
{
    // Start is called before the first frame update
    [SceneName]
    public string SceneName;
    public List<TileProperty> TileProperties;
}
