using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadCreditsScene : MonoBehaviour
{
    public void LoadCredits()
    {
        AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        Destroy(audioSource);
        SceneManager.LoadScene("CreditsScene",LoadSceneMode.Single);
    }
}
