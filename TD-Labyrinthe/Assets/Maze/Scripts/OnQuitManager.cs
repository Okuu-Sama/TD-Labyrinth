using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The following class define the behaviour of the game when closing the game
public class OnQuitManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        // We reset the score of the player upon closing the game
        int val = 0;
        if (Application.isEditor)
        {
            System.IO.File.WriteAllText("Assets/Maze/Data/bananas.txt", val.ToString());
        }
        else
        {
            System.IO.File.WriteAllText(Application.streamingAssetsPath + "banana.txt", val.ToString());

        }
    }
}
