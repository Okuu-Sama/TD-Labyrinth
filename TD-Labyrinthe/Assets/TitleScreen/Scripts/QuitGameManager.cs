using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class define the behaviour of the game when quitting in the Title Screen
public class QuitGameManager : MonoBehaviour
{
    // This method define the behaviour of the button used to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
