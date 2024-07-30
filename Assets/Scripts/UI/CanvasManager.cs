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

    [SerializeField]TextMeshProUGUI loggerCountTxt;
    [SerializeField]TextMeshProUGUI maxLoggerTxt;

    private Camera cam;

    public static CanvasManager instance;

    public Button addLoggerButton; //
    public Button removeLoggerButton; //

    private LoggerCamp loggerCamp; //


    private void Awake()
    {
        if(instance != null) 
        {
             Debug.Log("Il y a plus d'une instance de CanvasManager dans la scène !");
             return;
        }

        instance = this;

    }

    private void Start()
    {
        cam = Camera.main;
        loggerCamp = null;
    }

    
    public void OpenLoggerCampPanel() 
    {
        //Update button
        addLoggerButton.onClick.RemoveAllListeners();
        addLoggerButton.onClick.AddListener(OnAddLoggerClick);
        removeLoggerButton.onClick.RemoveAllListeners();
        removeLoggerButton.onClick.AddListener(OnRemoveLoggerClick);

        //Update UI
        UpdateLoggerCampPanel();

        //Display UI
        if (!loggerCampPanel.activeInHierarchy) 
        {
            loggerCampPanel.SetActive(true);
        }
    }

    //Maj panel LoggerCamp selon donnée du bâtiment sélectionné
    public void UpdateLoggerCampPanel() 
    {
        if(loggerCamp != null) 
        {
            loggerCountTxt.text = loggerCamp.loggersCount.ToString();
            maxLoggerTxt.text = loggerCamp.maxLoggerCount.ToString();
        }

        
    }


    public void CloseLoggerCampPanel()
    {
        loggerCampPanel.SetActive(false);
    }

    public void CloseAllBuildingsPanel()
    {
        CloseLoggerCampPanel();
        //...
        //...
    }


    public void SelectBuilding(Transform _structure) 
    {
        //Si c'est un camp de bûcheron
        if (_structure.CompareTag("loggerCamp"))
        {
            loggerCamp = _structure.GetComponent<LoggerCamp>();
            OpenLoggerCampPanel();
        }

    }

    void OnAddLoggerClick() 
    {
        if (loggerCamp != null) 
        {
            loggerCamp.AddLoggers(1);
        }
        UpdateLoggerCampPanel();


    }

    void OnRemoveLoggerClick()
    {
        if(loggerCamp != null) 
        {
            loggerCamp.RemoveLoggers(1);
        }
        UpdateLoggerCampPanel();

    }

    public void UpdateLoggerCamp(LoggerCamp _loggerCamp) 
    {
        loggerCamp = _loggerCamp;
    }
}
