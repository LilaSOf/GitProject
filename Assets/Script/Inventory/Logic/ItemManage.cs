using MFarm.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManage : MonoBehaviour
{
    // Start is called before the first frame update
    public Item itemPrefab;
    private Transform ItemParent;
    private void Start()
    {
        ItemParent = GameObject.FindWithTag("itemParent").transform;
    }
    public void OnEnable()
    {
        EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
    }
    private void OnDisable()
    {
        EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
    }
    private void OnInstantiateItemInScene(int id, Vector3 pos)
    {
        var it = Instantiate(itemPrefab,pos,Quaternion.identity,ItemParent);
        it.itemID = id;
    }


}
