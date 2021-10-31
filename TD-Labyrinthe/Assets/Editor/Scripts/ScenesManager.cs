using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class ScenesManager : EditorWindow
{
    private List<SceneData> listOfScene = null;

    [MenuItem("Tools/Scenes Manager")]
    static void ShowWindow()
    {
        ScenesManager window = (ScenesManager)GetWindow(typeof(ScenesManager),false,"Scenes Manager");
        
    }

    private void OnGUI()
    {
        
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal("");
        GUILayout.Label("Make sure all scenes are in the build: ");
        if(GUILayout.Button("Check build"))
        {
            GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
        GUILayout.EndHorizontal();

        //qqqqqqqqqqqqqqqqqqq

        GUILayout.BeginHorizontal("");
        GUILayout.Label("Select scenes to load: ");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh list"))
        {
            RefreshContent();
        }
        GUILayout.EndHorizontal();

        //GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.BeginScrollView(new Vector2(), GUILayout.Width(150), GUILayout.Height(250));

        if(listOfScene != null)
        {
            foreach(var scene in listOfScene)
            {
                if(scene.isOpen = GUILayout.Toggle(scene.isOpen, scene.sceneName))
                {
                    if(scene.displayNext)
                    {
                        EditorSceneManager.OpenScene(scene.scenePath, OpenSceneMode.Additive);
                        scene.displayNext = false;
                    }
                        
                }
                else
                {
                    if(EditorSceneManager.sceneCount == 1 && !scene.displayNext)
                    {
                        
                        GetWindow<ScenesManager>().ShowNotification(new GUIContent("ERROR: can't close the last opened scene"));
                        scene.isOpen = true;
                    }
                    else
                    {
                        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName(scene.sceneName),true);
                        scene.displayNext = true;
                    }
                }
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        //GUILayout.FlexibleSpace();
    }

    void RefreshContent()
    {
        if(listOfScene is null)
            listOfScene = new List<SceneData>();
        listOfScene.Clear();
        foreach(var scene in EditorBuildSettings.scenes)
        {
            Debug.Log(scene.path);
            listOfScene.Add(new SceneData(Path.GetFileNameWithoutExtension(scene.path),
                                          scene.path,
                                          false,
                                          true));
        }

        int sceneIndex = 0;
        for(int i = 0; i < EditorSceneManager.sceneCount;++i)
        {
            sceneIndex = EditorSceneManager.GetSceneAt(i).buildIndex;
            if (sceneIndex >= 0 && sceneIndex < EditorSceneManager.sceneCount)
            {
                listOfScene[sceneIndex].isOpen = true;
                listOfScene[sceneIndex].displayNext = false;
            }
        }
    }

    [MenuItem("Assets/Scene/Add to build", false)]
    static void AddToBuild()
    {
        SceneAsset selectedObj = (Selection.activeObject is SceneAsset) ? (SceneAsset)Selection.activeObject : null;
        Debug.Log(selectedObj.name);
        if (selectedObj is null) return;
        if (EditorBuildSettings.scenes.Any(s => s.path == AssetDatabase.GetAssetPath(selectedObj)))
        {
            GetWindow<EditorWindow>().ShowNotification(new GUIContent("ERROR: Scene is already in build"));
            return;
        }

        var listOfScene = EditorBuildSettings.scenes.ToList();
        listOfScene.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(selectedObj), true));
        EditorBuildSettings.scenes = listOfScene.ToArray();

    }
    [MenuItem("Assets/Scene/Add to build", true)] 
    static bool AddToBuildValidate()
    {
        if (Selection.activeObject is SceneAsset) return true;
        return false;
    }

}
