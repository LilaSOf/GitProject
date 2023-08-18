using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MapData",menuName ="Map/MapData")]
public class MapData_SO :ScriptableObject
{
    // Start is called before the first frame update
    [SceneName]
    public string SceneName;
    [Header("��ͼ��Ϣ")]
    public int gridWidth;
    public int gridHeight;

    [Header("���½�ԭ��")]
    public int gridX;
    public int gridY;
    public List<TileProperty> TileProperties;
}
