using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnQuitManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        int val = 0;
        System.IO.File.WriteAllText("Assets/Maze/Data/bananas.txt", val.ToString());
    }
}
