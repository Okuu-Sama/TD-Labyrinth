using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// UNUSED CLASS
// This class define the behaviour of the button to load the Maze scene
public class LoadMazeScene : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("MazeScene",LoadSceneMode.Single);
    }
}
