using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
            Destroy(audioSource);
            SceneManager.LoadScene("Title_screen", LoadSceneMode.Single);
        }
    }
}
