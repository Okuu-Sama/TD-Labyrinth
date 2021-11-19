using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;
using System.IO;

// The following class define a new tab available in the editor to help in scene management
public class ScenesManager : EditorWindow
{
    private List<SceneData> listOfScene = null;

    // The tab is accessible from the Tools tab of Unity
    [MenuItem("Tools/Scenes Manager")]
    static void ShowWindow()
    {
        ScenesManager window = (ScenesManager)GetWindow(typeof(ScenesManager),false,"Scenes Manager");
        
    }

    // This method define the content of the tab
    private void OnGUI()
    {
        
        GUILayout.BeginVertical();

        // All scenes must be in the build in order to be displayable in the tab
        GUILayout.BeginHorizontal("");
        GUILayout.Label("Make sure all scenes are in the build: ");
        if(GUILayout.Button("Check build"))
        {
            GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
        GUILayout.EndHorizontal();

        // We fetch the list of scenes available to be loaded
        GUILayout.BeginHorizontal("");
        GUILayout.Label("Select scenes to load: ");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Refresh list"))
        {
            RefreshContent();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.BeginScrollView(new Vector2(), GUILayout.Width(150), GUILayout.Height(250));

        // Displaying the list of all the scenes
        // we can toggle on a scene to open it with the current one 
        // or toggle a scene of to remove it from the currently open scenes
        if(listOfScene != null)
        {
            foreach(var scene in listOfScene)
            {
                // If the scene is active then it is already toggled on
                // it is toggled off otherwise
                if(scene.isOpen = GUILayout.Toggle(scene.isOpen, scene.sceneName))
                {
                    // if the scene is pressed to be toggled on we load it and set it as toggled
                    if(scene.displayNext)
                    {
                        EditorSceneManager.OpenScene(scene.scenePath, OpenSceneMode.Additive);
                        scene.displayNext = false;
                    }
                }
                else
                {
                    // If we are at the last scene loaded and we wnat to unload the current scene we get an error message
                    // We set the current scene as open again
                    if(EditorSceneManager.sceneCount == 1 && !scene.displayNext)
                    {
                        
                        GetWindow<ScenesManager>().ShowNotification(new GUIContent("ERROR: can't close the last opened scene"));
                        scene.isOpen = true;
                    }
                    else
                    {
                        // If the scene is not the last scene we unload it
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

    // Method used to refresh the content of the list of available scene
    void RefreshContent()
    {
        if(listOfScene is null)
            listOfScene = new List<SceneData>();
        listOfScene.Clear();
        foreach(var scene in EditorBuildSettings.scenes)
        {
            Debug.Log(scene.path);
            if (EditorSceneManager.GetActiveScene().path == scene.path)
            {
                // If we find the current scene in the list of scene we set it as already opened
                listOfScene.Add(new SceneData(Path.GetFileNameWithoutExtension(scene.path),
                                          scene.path,
                                          true,
                                          false));
            }
            else
            {
                listOfScene.Add(new SceneData(Path.GetFileNameWithoutExtension(scene.path),
                                          scene.path,
                                          false,
                                          true));
            }
        }

        // We put the already opened scene as opened for the toggle list
        int sceneIndex = 0;
        for(int i = 0; i < EditorSceneManager.sceneCount;++i)
        {
            sceneIndex = EditorSceneManager.GetSceneAt(i).buildIndex;

            if (sceneIndex >= 0 && sceneIndex < EditorSceneManager.sceneCount)
            {
                Debug.Log("reached");
                listOfScene[sceneIndex].isOpen = true;
                listOfScene[sceneIndex].displayNext = false;
            }
        }
    }

    // The following method define a contextual tab that only appear when right clicking on a scene object
    // we can then add the scene in the build list
    [MenuItem("Assets/Scene/Add to build", false)]
    static void AddToBuild()
    {
        SceneAsset selectedObj = (Selection.activeObject is SceneAsset) ? (SceneAsset)Selection.activeObject : null;
        Debug.Log(selectedObj.name);
        if (selectedObj is null) return;
        
        // If a scene is already in the build we display an error
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
