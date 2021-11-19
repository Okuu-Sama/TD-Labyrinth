using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class define the behaviour of the button available in the title screen
public class SceneLoader : MonoBehaviour
{
    // This method load the Maze using the start game button
    public void LoadGame()
    {
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Single);
    }

    // This method load the credits scene using the credits button
    public void LoadCredits()
    {
        AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        Destroy(audioSource);
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }
}
