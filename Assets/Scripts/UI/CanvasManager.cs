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

    //[SerializeField] TextMeshProUGUI stoneMinerCountTxt;
    //[SerializeField] TextMeshProUGUI maxStoneMinerTxt;
    //-----------------------------------

    //------------- PANELS ---------------
    [SerializeField] GameObject buildingPanel;
    [SerializeField] GameObject loggerCampPanel;
    //[SerializeField] GameObject stoneQuarryPanel;
    //-----------------------------------

    //------------- BUTTONS ---------------
    [SerializeField] Button openBuildingPanelButton;

    [SerializeField] Button addLoggerButton;
    [SerializeField] Button removeLoggerButton;

    //[SerializeField] Button addStoneMinerButton;
    //[SerializeField] Button removeStoneMinerButton;

    //-------------------------------------

    //------------- SLIDERS ----------------
    [SerializeField] Slider loggerCampSlider;
    //[SerializeField] Slider stoneQuarrySlider;
    //-------------------------------------

    private LoggerCamp loggerCamp;
    //private StoneQuarry stoneQuarry; //OK


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


    public void SelectBuilding(Transform _structure)
    {
        loggerCamp = null;
        //stoneQuarry = null; //OK

        //Si c'est un camp de bûcheron
        if (_structure.CompareTag("loggerCamp"))
        {
            loggerCamp = _structure.GetComponent<LoggerCamp>();
            OpenLoggerCampPanel();
        }
        else if (_structure.CompareTag("stoneQuarry"))
        {
            //stoneQuarry = _structure.GetComponent<StoneQuarry>(); //OK
            //OpenStoneQuarryPanel(); //OK
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
        //CloseStoneQuarryPanel(); //OK
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


    /*
    public void OpenLoggerCampPanel()
    {
        //Update button
        addStoneMinerButton.onClick.RemoveAllListeners();
        addStoneMinerButton.onClick.AddListener(OnAddStoneMinerClick);
        removeStoneMinerButton.onClick.RemoveAllListeners();
        removeStoneMinerButton.onClick.AddListener(OnRemoveStoneMinerClick);

        //Update UI
        UpdateStoneQuarryPanel();

        //Display UI
        if (!stoneQuarryPanel.activeInHierarchy)
        {
            stoneQuarryPanel.SetActive(true);
        }
    }






    //Maj panel LoggerCamp selon donnée du bâtiment sélectionné
    public void UpdateStoneQuarryPanel()
    {
        if (stoneQuarry != null)
        {
            //Update text
            stoneMinerCountTxt.text = stoneQuarry.stoneMinerCount.ToString();
            maxStoneMinerTxt.text = stoneQuarry.maxStoneMiner.ToString();

            //Update health bar
            BuildingData buildingData = stoneQuarry.GetComponent<BuildingData>();

            if (buildingData != null)
            {
                int _maxHealth = buildingData.maxHealth;
                int _currentHealth = buildingData.currentHealth;

                stoneQuarrySlider.maxValue = _maxHealth;
                stoneQuarrySlider.value = _currentHealth;
            }

        }


    }
    */






    /*
    public void CloseStoneQuarryPanel()
    {
        stoneQuarryPanel.SetActive(false);
    }
    */


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

    /*
    
    void OnAddStoneMinerClick() //OK
    {
        if (stoneQuarry != null)
        {
            stoneQuarry.AddStoneMiner(1);
        }
        UpdateStoneQuarryPanel();


    }

    void OnRemoveStoneMinerClick() //OK
    {
        if (stoneQuarry != null)
        {
            stoneQuarry.RemoveStoneMiner(1);
        }
        UpdateStoneQuarryPanel();

    }

    */


}
