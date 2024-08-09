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

    //COST (UI) -> Building Panel
    [SerializeField] TextMeshProUGUI home_woodCost_txt;
    [SerializeField] TextMeshProUGUI home_stoneCost_txt;

    [SerializeField] TextMeshProUGUI loggerCamp_woodCost_txt;
    [SerializeField] TextMeshProUGUI loggerCamp_stoneCost_txt;

    [SerializeField] TextMeshProUGUI stoneMinerHut_woodCost_txt;
    [SerializeField] TextMeshProUGUI stoneMinerHut_stoneCost_txt;

    [SerializeField] TextMeshProUGUI barrack_woodCost_txt;
    [SerializeField] TextMeshProUGUI barrack_stoneCost_txt;
    //-----------------------------------

    //------------- PANELS ---------------
    [SerializeField] GameObject buildingPanel;
    [SerializeField] GameObject loggerCampPanel;
    [SerializeField] GameObject stoneMinerHutPanel;
    [SerializeField] GameObject barrackPanel;
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
    //private Barrack barrack;


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

        UpdateTextCostOnBuildingPanel();
    }

    /*
    private void Update()
    {
        if(barrack != null) 
        {
            barrack
        }
    }
    */

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
        else if (_structure.CompareTag("barrack")) 
        { 
            OpenBarrackPanel();
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
        CloseBarrackPanel();
    }

    public void UpdateTextCostOnBuildingPanel() //Appelée une fois, au début (les coûts ne changent pas durant la partie)
    {
        //Home
        home_woodCost_txt.text = BuildingManager.instance.homePrefab.GetComponent<BuildingData>().woodCost.ToString();
        home_stoneCost_txt.text = BuildingManager.instance.homePrefab.GetComponent<BuildingData>().stoneCost.ToString();

        //Logger camp
        loggerCamp_woodCost_txt.text = BuildingManager.instance.loggerCampPrefab.GetComponent<BuildingData>().woodCost.ToString();
        loggerCamp_stoneCost_txt.text = BuildingManager.instance.loggerCampPrefab.GetComponent<BuildingData>().stoneCost.ToString();

        //Stone miner's hut
        stoneMinerHut_woodCost_txt.text = BuildingManager.instance.stoneMinerHutPrefab.GetComponent<BuildingData>().woodCost.ToString();
        stoneMinerHut_stoneCost_txt.text = BuildingManager.instance.stoneMinerHutPrefab.GetComponent<BuildingData>().stoneCost.ToString();

        //Barrack
        barrack_woodCost_txt.text = BuildingManager.instance.barrackPrefab.GetComponent<BuildingData>().woodCost.ToString();
        barrack_stoneCost_txt.text = BuildingManager.instance.barrackPrefab.GetComponent<BuildingData>().stoneCost.ToString();
    }


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


    //--------- BARRACK ------------


    //---- PANELS
    public void OpenBarrackPanel()
    {
        //Update button
        //addLoggerButton.onClick.RemoveAllListeners();
        //addLoggerButton.onClick.AddListener(OnAddLoggerClick);

        //Update UI
        //UpdateBarrackPanel();

        //Display UI
        if (!barrackPanel.activeInHierarchy)
        {
            barrackPanel.SetActive(true);
        }
    }

    public void CloseBarrackPanel()
    {
        barrackPanel.SetActive(false);
    }

    /*
    public void UpdateBarrackPanel()
    {
        BuildingData buildingData = barrack.

        //Update health bar
        if (_buildingData != null)
        {
            int _maxHealth = _buildingData.maxHealth;
            int _currentHealth = _buildingData.currentHealth;

            stoneMinerHutSlider.maxValue = _maxHealth;
            stoneMinerHutSlider.value = _currentHealth;
        }
    }
    */
  

}
