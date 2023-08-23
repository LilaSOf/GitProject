using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManage : Singleton<NPCManage>
{
    // Start is called before the first frame update
   public List<NPCPosition > NPCPositions = new List<NPCPosition>();

    public ChangeScenePath_SO sceneRoute;

    public Dictionary<string,SceneRoute> sceneRouteDict = new Dictionary<string,SceneRoute>();//字典内容储存场景名和对应信息

    protected override void Awake()
    {
        base.Awake();
        InitSceneRoute();
    }
    /// <summary>
    /// 初始化字典
    /// </summary>
    private void InitSceneRoute()
    {
        if(sceneRoute != null)
        {
            foreach(var sceneRouteData in sceneRoute.sceneRoutesList)
            {
                string key = sceneRouteData.fromSceneName + sceneRouteData.goToSceneName;
                if (sceneRouteDict.ContainsKey(key))
                    continue;
                else 
                {
                    sceneRouteDict.Add(key, sceneRouteData);
                }
            }
        }
        foreach(var dir in sceneRouteDict.Keys)
        {
            Debug.Log(dir);
        }
    }

   /// <summary>
   /// 通过from场景名和go场景名组成的key在字典中找到对应路径
   /// </summary>
   /// <param name="key"></param>
   /// <returns></returns>
   public SceneRoute GetSceneRouteFormKey(string fromScene,string toScene)
    {
        return sceneRouteDict[(fromScene + toScene).Trim()];
    }
}
