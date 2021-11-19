using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// UNUSED CLASS
// This class define the behaviour of the button to load the credits scene
public class LoadCreditsScene : MonoBehaviour
{
    public void LoadCredits()
    {
        AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        Destroy(audioSource);
        SceneManager.LoadScene("CreditsScene",LoadSceneMode.Single);
    }
}
