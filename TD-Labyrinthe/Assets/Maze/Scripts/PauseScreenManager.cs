using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MonoBehaviour
{
    public GameObject pauseScreen;
    private void Update()
    {
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

    public void ContinueGame()
    {
        pauseScreen.SetActive(false);
    }

    public void ResetGameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
    }

    public void OpenSettings()
    {

    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("Title_screen", LoadSceneMode.Single);
    }

}
