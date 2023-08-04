using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    int sceneIndex = -1;
    GUIContent[] sceneNames;

    readonly string[] sceneNameSplit = { "/", ".unity" };

    // Start is called before the first frame update
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (EditorBuildSettings.scenes.Length == 0) return;
        if (sceneIndex == -1)
            GetSceneName(property);
        int OldIndex = sceneIndex;
       sceneIndex = EditorGUI.Popup(position,label,sceneIndex,sceneNames);
        if(sceneIndex != OldIndex)
        {
            property.stringValue = sceneNames[sceneIndex].text;
        }
    }
    private void GetSceneName(SerializedProperty property)
    {
        var scenes = EditorBuildSettings.scenes;
        //初始化数据
        sceneNames = new GUIContent[scenes.Length];

        for(int i = 0;i< scenes.Length; i++)
        {
            string path = scenes[i].path;
            string[] splitePath = path.Split(sceneNameSplit,System.StringSplitOptions.RemoveEmptyEntries);

            string sceneName = "";
            if(splitePath.Length > 0)
            {
                sceneName = splitePath[splitePath.Length-1];
            }
            else
            {
                sceneName = "(Delete Scene)";
            }
            sceneNames[i] = new GUIContent(sceneName);
        }
        if(sceneNames.Length ==0)
        {
            sceneNames = new[] { new GUIContent("Check You Build Settings") };
        }
        if(!string.IsNullOrEmpty(property.stringValue))
        {
            bool nameFound = false;
            for(int i = 0;i<sceneNames.Length;i++)
            {
                if (sceneNames[i].text == property.stringValue)
                {
                    nameFound = true;
                   sceneIndex = i;
                    break;
                }
            }
            if(!nameFound)
            {
                sceneIndex = 0;
            }
        }
        else
        {
            sceneIndex = 0;
        }
        property.stringValue = sceneNames[sceneIndex].text;
    }
}
