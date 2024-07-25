using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script en cours/non terminé

public class CanvasManager : MonoBehaviour
{
    //LOGGER CAMP
    public GameObject loggerCampPanel;
    public TextMeshProUGUI loggerCountTxt;
    public TextMeshProUGUI maxLoggerTxt;


    public void OpenLoggerCampPanel(LoggerCamp _loggercamp) 
    {
        if (!loggerCampPanel.activeInHierarchy) 
        {
            //Display
            loggerCampPanel.SetActive(true);
        }
    }


    public void CloseLoggerCampPanel()
    {
        loggerCampPanel.SetActive(false);
    }

    

}
