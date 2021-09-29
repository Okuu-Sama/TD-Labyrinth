using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScenesManager : EditorWindow
{
    [MenuItem("Tools/Scenes Manager")]
    static void ShowWindow()
    {
        ScenesManager window = (ScenesManager)GetWindow(typeof(ScenesManager),false,"Scenes Manager");
        
    }

    private void OnGUI()
    {
        
    }

}
