using MFarm.Inventory;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MFarm.Inventory
{
    public class ItemManage : MonoBehaviour
    {
        // Start is called before the first frame update
        public Item itemPrefab;
        [SerializeField]
        private Transform ItemParent;

        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();
        private void Start()
        {
            ItemParent = GameObject.FindWithTag("itemParent").transform;
        }
        public void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforeFade += OnBeforFade;
            EventHandler.AfterFade += OnAfterFade;
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeFade -= OnBeforFade;
            EventHandler.AfterFade -= OnAfterFade;
        }

        private void OnBeforFade(string SceneName)
        {
            GetSceneItem(SceneName);
        }

        private void OnAfterFade(string SceneName)
        {
            ItemParent = GameObject.FindWithTag("itemParent").transform;
            RecreatedAllItem(SceneName);
        }

        private void OnInstantiateItemInScene(int id, Vector3 pos)
        {
            var it = Instantiate(itemPrefab, pos, Quaternion.identity, ItemParent);
            it.itemID = id;
        }

        /// <summary>
        /// 储存场景中的所有物体
        /// </summary>
        private void GetSceneItem(string SceneName)
        {
            List<SceneItem> currentSceneItem = new List<SceneItem>();
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem();
                sceneItem.itemID = item.itemID;
                sceneItem.position = new SerializableVector3(item.transform.position);
                currentSceneItem.Add(sceneItem);
            }
            if (sceneItemDict.ContainsKey(SceneName))
            {
                sceneItemDict[SceneName] = currentSceneItem;
            }
            else
            {
                sceneItemDict.Add(SceneName, currentSceneItem);
            }
        }

        /// <summary>
        /// 重新创建场景中的所有物体
        /// </summary>
        private void RecreatedAllItem(string SceneName)
        {
            List<SceneItem> sceneitem = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(SceneName, out sceneitem))
            {
                if (sceneitem != null)
                {
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in sceneItemDict[SceneName])
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, ItemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}
