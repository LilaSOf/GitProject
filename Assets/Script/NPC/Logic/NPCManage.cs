using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManage : Singleton<NPCManage>
{
    // Start is called before the first frame update
   public List<NPCPosition > NPCPositions = new List<NPCPosition>();

    public ChangeScenePath_SO sceneRoute;

    public Dictionary<string,SceneRoute> sceneRouteDict = new Dictionary<string,SceneRoute>();//�ֵ����ݴ��泡�����Ͷ�Ӧ��Ϣ

    protected override void Awake()
    {
        base.Awake();
        InitSceneRoute();
    }
    /// <summary>
    /// ��ʼ���ֵ�
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
   /// ͨ��from��������go��������ɵ�key���ֵ����ҵ���Ӧ·��
   /// </summary>
   /// <param name="key"></param>
   /// <returns></returns>
   public SceneRoute GetSceneRouteFormKey(string fromScene,string toScene)
    {
        return sceneRouteDict[(fromScene + toScene).Trim()];
    }
}
