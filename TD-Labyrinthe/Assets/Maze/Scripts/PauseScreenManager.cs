using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The following class let us define the behaviour of the pause screen
public class PauseScreenManager : MonoBehaviour
{
    // The canvas representing the pause screen is stored in a gameobject
    // to easily toggle on and off the UI
    public GameObject pauseScreen;
    private void Update()
    {
        // Pressing escape let us toggle on and off the pause screen
        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            if (pauseScreen.activeSelf)
            {
                pauseScreen.SetActive(false);
            }
            else
            {
                pauseScreen.SetActive(true);
            }
        }
    }

    // This method define the behaviour of the Continue Game button
    public void ContinueGame()
    {
        pauseScreen.SetActive(false);
    }

    // This method define the behaviour of the Restart Game button
    public void ResetGameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
    }

    // This method define the behaviour of the Return to Title Screen button
    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("Title_screen", LoadSceneMode.Single);
    }

}
