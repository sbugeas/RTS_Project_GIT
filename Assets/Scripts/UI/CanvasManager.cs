using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

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

    [SerializeField] TextMeshProUGUI recruitedUnitNameTxt;

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

    [SerializeField] Button recruitSoldierButton;

    //-------------------------------------

    //------------- SLIDERS ----------------
    [SerializeField] Slider loggerCampSlider;
    [SerializeField] Slider stoneMinerHutSlider;
    [SerializeField] Slider recruitmentBarSlider;
    //-------------------------------------

    private LoggerCamp loggerCamp;
    private StoneMinerHut stoneMinerHut;
    private Barrack barrack;


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
        stoneMinerHut = null;
        barrack = null;

        UpdateTextCostOnBuildingPanel();
    }

   
    private void Update()
    {
        //----------------------------------------------------------------------- CHANGER CAR PAS PROPRE/PERFORMANT
        if (barrack != null) //Caserne sélectionnée
        {
            if(barrack.isRecruiting) //Si en recrutement
            {
                //Update progressbar
                float recruitmentTime = barrack.timeToRecruitSoldier;
                float curTime = barrack.curTime;

                recruitmentBarSlider.value = curTime / recruitmentTime;
            }

            if(Input.GetMouseButtonDown(1)) //Clique droit
            {
                Transform _rallyFlag = barrack.transform.Find("rallyFlag").transform;

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //On détécte un bâtiment
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, UnitSelectionManager.instance.buildingLayerP1))
                {
                    NavMeshHit _hit;

                    //Si zone naviguable près du point de ralliement (rallyPosition)
                    if (NavMesh.SamplePosition(hit.point, out _hit, 100f, NavMesh.AllAreas))
                    {
                        _rallyFlag.position = _hit.position;
                    }
                }
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, UnitSelectionManager.instance.ground)) //On détecte le sol
                {
                    _rallyFlag.position = hit.point; //test
                }
            }
        }
        //----------------------------------------------------------------------- CHANGER CAR PAS PROPRE/PERFORMANT





    }


    //------------- GENERAL --------------

    public void SelectBuilding(Transform _structure)
    {
        //logger camp
        if (_structure.CompareTag("loggerCamp"))
        {
            loggerCamp = _structure.GetComponent<LoggerCamp>();
            OpenLoggerCampPanel();
        }
        else if (_structure.CompareTag("stoneMinerHut")) //stone miner's hut
        {
            stoneMinerHut = _structure.GetComponent<StoneMinerHut>();
            OpenStoneMinerHutPanel();
        }
        else if (_structure.CompareTag("barrack")) //barrack
        {
            barrack = _structure.GetComponent<Barrack>();

            barrack.isSelected = true;
            barrack.transform.Find("rallyFlag").gameObject.SetActive(true); //ok
            OpenBarrackPanel();
        }
    }

    public void DeselectBuildings() /////
    {
        CloseAllOpenedPanel(); /////

        //Reset
        loggerCamp = null;
        stoneMinerHut = null;
        barrack = null;
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

        CloseBarrackPanel();
        CloseLoggerCampPanel();
        CloseStoneMinerHutPanel();
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
        if (barrack.isRecruiting) 
        {
            DisplayRecruitmentInfos();
        }
        else 
        {
            ClearRecruitmentInfos();
        }

        //Update button
        recruitSoldierButton.onClick.RemoveAllListeners();
        recruitSoldierButton.onClick.AddListener(OnRecruitSoldierClick);


        //Display UI
        if (!barrackPanel.activeInHierarchy)
        {
            barrackPanel.SetActive(true);
        }
    }

    public void CloseBarrackPanel()
    {
        barrackPanel.SetActive(false);
        ClearRecruitmentInfos();

        if (barrack != null)
        {
            barrack.transform.Find("rallyFlag").gameObject.SetActive(false);
            barrack.isSelected = false;
        }
    }

    
    public void DisplayRecruitmentInfos()
    {
        //reset slider
        recruitmentBarSlider.value = 0.0f;

        //display
        recruitmentBarSlider.gameObject.SetActive(true);
        recruitedUnitNameTxt.enabled = true;

        //Disable recruitment button
        recruitSoldierButton.interactable = false;
    }

    public void ClearRecruitmentInfos()
    {
        recruitmentBarSlider.gameObject.SetActive(false);
        recruitedUnitNameTxt.enabled = false;

        //Enable recruitment button
        recruitSoldierButton.interactable = true;

    }

    //---- BUTTONS
    void OnRecruitSoldierClick()
    {
        if (barrack != null)
        {
            barrack.RecruitSoldier();
        }
    }


}
