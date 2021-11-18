using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanelManager : MonoBehaviour
{
    public void RemovePanel()
    {
        GameObject panelToRemove = GameObject.Find("HelpPanel");
        panelToRemove.SetActive(false);
    }
}
