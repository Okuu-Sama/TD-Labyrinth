using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class define the behaviour of the credits scene when pressing certain button
public class InputManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // When pressing the escape button we load the title screen scene
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
            Destroy(audioSource);
            SceneManager.LoadScene("Title_screen", LoadSceneMode.Single);
        }
    }
}
