using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script en cours/non terminé
public class CanvasManager : MonoBehaviour
{
    private Camera cam;
    public static CanvasManager instance;

    //-------------- TEXTS ---------------
    [SerializeField] TextMeshProUGUI loggerCountTxt;
    [SerializeField] TextMeshProUGUI maxLoggerTxt;

    [SerializeField] TextMeshProUGUI stoneMinerCountTxt;
    [SerializeField] TextMeshProUGUI maxStoneMinerTxt;
    //-----------------------------------

    //------------- PANELS ---------------
    [SerializeField] GameObject buildingPanel;
    [SerializeField] GameObject loggerCampPanel;
    [SerializeField] GameObject stoneMinerHutPanel;
    //-----------------------------------

    //------------- BUTTONS ---------------
    [SerializeField] Button openBuildingPanelButton;

    [SerializeField] Button addLoggerButton;
    [SerializeField] Button removeLoggerButton;

    [SerializeField] Button addStoneMinerButton;
    [SerializeField] Button removeStoneMinerButton;

    //-------------------------------------

    //------------- SLIDERS ----------------
    [SerializeField] Slider loggerCampSlider;
    [SerializeField] Slider stoneMinerHutSlider;
    //-------------------------------------

    private LoggerCamp loggerCamp;
    private StoneMinerHut stoneMinerHut;


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


    //------------- GENERAL --------------

    public void SelectBuilding(Transform _structure)
    {
        loggerCamp = null;
        stoneMinerHut = null;

        //Si c'est un camp de bûcheron
        if (_structure.CompareTag("loggerCamp"))
        {
            loggerCamp = _structure.GetComponent<LoggerCamp>();
            OpenLoggerCampPanel();
        }
        else if (_structure.CompareTag("stoneMinerHut")) //Si c'est une cabane de mineur de pierre
        {
            stoneMinerHut = _structure.GetComponent<StoneMinerHut>();
            OpenStoneMinerHutPanel();
        }
    }

    public void OpenBuildingPanel()
    {
        buildingPanel.SetActive(true);
    }

    public void CloseBuildingPanel()
    {
        buildingPanel.SetActive(false);
    }

    public void DisplayBuildingButton(bool val)
    {
        openBuildingPanelButton.gameObject.SetActive(val);
    }

    public void CloseAllOpenedPanel()
    {
        if (BuildingManager.instance.buildingInstantiated == false)
        {
            CloseBuildingPanel();
            DisplayBuildingButton(true);
        }


        CloseLoggerCampPanel();
        CloseStoneMinerHutPanel();
    }

    //------------- GENERAL --------------



    //--------- LOGGER CAMP ------------

    //---- PANELS
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

    //---- BUTTONS

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
        if (loggerCamp != null)
        {
            loggerCamp.RemoveLoggers(1);
        }
        UpdateLoggerCampPanel();
    }

    //--------- LOGGER CAMP ------------



    //--------- STONE MINER HUT ------------

    //---- PANELS

    public void OpenStoneMinerHutPanel()
    {
        //Update button
        addStoneMinerButton.onClick.RemoveAllListeners();
        addStoneMinerButton.onClick.AddListener(OnAddStoneMinerClick);
        removeStoneMinerButton.onClick.RemoveAllListeners();
        removeStoneMinerButton.onClick.AddListener(OnRemoveStoneMinerClick);

        //Update UI
        UpdateStoneMinerHutPanel();

        //Display UI
        if (!stoneMinerHutPanel.activeInHierarchy)
        {
            stoneMinerHutPanel.SetActive(true);
        }
    }


    //Maj panel StoneMinerHut selon données du bâtiment sélectionné
    public void UpdateStoneMinerHutPanel()
    {
        if (stoneMinerHut != null)
        {
            //Update text
            stoneMinerCountTxt.text = stoneMinerHut.stoneMinerCount.ToString();
            maxStoneMinerTxt.text = stoneMinerHut.maxStoneMiner.ToString();

            //Update health bar
            BuildingData buildingData = stoneMinerHut.GetComponent<BuildingData>();

            if (buildingData != null)
            {
                int _maxHealth = buildingData.maxHealth;
                int _currentHealth = buildingData.currentHealth;

                stoneMinerHutSlider.maxValue = _maxHealth;
                stoneMinerHutSlider.value = _currentHealth;
            }

        }


    }

    public void CloseStoneMinerHutPanel()
    {
        stoneMinerHutPanel.SetActive(false);
    }

    //---- BUTTONS

    void OnAddStoneMinerClick()
    {
        if (stoneMinerHut != null)
        {
            stoneMinerHut.AddStoneMiners(1);
        }
        UpdateStoneMinerHutPanel();
    }
    void OnRemoveStoneMinerClick()
    {
        if (stoneMinerHut != null)
        {
            stoneMinerHut.RemoveStoneMiners(1);
        }
        UpdateStoneMinerHutPanel();
    }


    //--------- STONE MINER HUT ------------

    
    
    




}
