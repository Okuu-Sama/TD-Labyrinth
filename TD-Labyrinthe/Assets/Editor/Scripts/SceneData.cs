using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    public string sceneName;
    public string scenePath;
    public bool isOpen;
    public bool displayNext;

    public SceneData(string _sceneName, string _scenePath, bool _isOpen, bool _displayNext)
    {
        sceneName = _sceneName;
        scenePath = _scenePath;
        isOpen = _isOpen;
        displayNext = _displayNext;
    }

}
