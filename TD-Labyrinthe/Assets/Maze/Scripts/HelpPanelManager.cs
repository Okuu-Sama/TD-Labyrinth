using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The following class contain all the methods related to the UI control of the panel guiding the player
public class HelpPanelManager : MonoBehaviour
{
    public void RemovePanel()
    {
        GameObject panelToRemove = GameObject.Find("HelpPanel");
        panelToRemove.SetActive(false);
    }
}
