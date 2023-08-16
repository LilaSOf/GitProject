using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<GameObject> poolPrefabList = new List<GameObject>();
    public List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    void Start()
    {
        CreatePool();
    }
    private void OnEnable()
    {
        EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
    }
    private void OnDisable()
    {
        EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
    }

 

    /// <summary>
    /// 生成对象池
    /// </summary>
    private void CreatePool()
    {
        foreach(var item in poolPrefabList)
        {
            Transform parent =new GameObject(item.name).transform;
            parent.SetParent(transform);

            var newPool = new ObjectPool<GameObject>
                (
                   () => GameObject.Instantiate(item,parent),
                   e => e.SetActive(true),
                   e => e.SetActive(false),
                   e=>Destroy(e)
                ) ;
           poolEffectList.Add(newPool);
        }
    }
    // Update is called once per frame

    private void OnParticleEffectEvent(ParticleType particleType, Vector3 pos)
    {
       var objPool = particleType switch
        {
            ParticleType.LeavesFall01 => poolEffectList[0],
            ParticleType.LeavesFall02 => poolEffectList[1],
            ParticleType.RockFall => poolEffectList[2],
            ParticleType.GrassFall => poolEffectList[3],
            _ => null
        };
       GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }
    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> objectPool,GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        objectPool.Release(obj);
    }
}
