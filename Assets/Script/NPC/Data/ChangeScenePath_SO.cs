using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="sceneRoute",menuName ="Map/sceneRoute")]
public class ChangeScenePath_SO : ScriptableObject
{
    // Start is called before the first frame update
    public List<SceneRoute> sceneRoutesList = new List<SceneRoute>();
}
