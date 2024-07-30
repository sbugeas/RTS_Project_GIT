using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script en cours/non termin�
public class CanvasManager : MonoBehaviour
{
    private Camera cam;
    public static CanvasManager instance;

    //------------- LOGGER CAMP ---------------
    [SerializeField] TextMeshProUGUI loggerCountTxt;
    [SerializeField] TextMeshProUGUI maxLoggerTxt;
    [SerializeField] Slider loggerCampSlider;

    public GameObject loggerCampPanel;

    public Button addLoggerButton;
    public Button removeLoggerButton;

    private LoggerCamp loggerCamp;
    //-----------------------------------------

    private void Awake()
    {
        if(instance != null) 
        {
             Debug.Log("Il y a plus d'une instance de CanvasManager dans la sc�ne !");
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

    //Maj panel LoggerCamp selon donn�e du b�timent s�lectionn�
    public void UpdateLoggerCampPanel() 
    {
        if(loggerCamp != null) 
        {
            //Update text
            loggerCountTxt.text = loggerCamp.loggersCount.ToString();
            maxLoggerTxt.text = loggerCamp.maxLoggerCount.ToString();

            //Update health bar
            BuildingData buildingData = loggerCamp.GetComponent<BuildingData>();

            if (buildingData != null) 
            {
                int _maxHealth = buildingData.maxHealth;
                int _currentHealth = buildingData.currentHealth;

                loggerCampSlider.maxValue = _maxHealth;
                loggerCampSlider.value = _currentHealth;
            }
            
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
        //Si c'est un camp de b�cheron
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
