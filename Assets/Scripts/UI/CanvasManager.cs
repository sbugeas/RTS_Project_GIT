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

    [SerializeField] TextMeshProUGUI goldMinerCountTxt; ///
    [SerializeField] TextMeshProUGUI maxGoldMinerTxt; ///

    [SerializeField] TextMeshProUGUI recruitedUnitNameTxt;

    [SerializeField] TextMeshProUGUI soldierCostLabel;
    [SerializeField] TextMeshProUGUI soldierCostVal;

    //COST (UI) -> Building Panel
    [SerializeField] TextMeshProUGUI home_woodCost_txt;
    [SerializeField] TextMeshProUGUI home_stoneCost_txt;

    [SerializeField] TextMeshProUGUI loggerCamp_woodCost_txt;
    [SerializeField] TextMeshProUGUI loggerCamp_stoneCost_txt;

    [SerializeField] TextMeshProUGUI stoneMinerHut_woodCost_txt;
    [SerializeField] TextMeshProUGUI stoneMinerHut_stoneCost_txt;

    [SerializeField] TextMeshProUGUI goldMinerHut_woodCost_txt; ///
    [SerializeField] TextMeshProUGUI goldMinerHut_stoneCost_txt; ///

    [SerializeField] TextMeshProUGUI barrack_woodCost_txt;
    [SerializeField] TextMeshProUGUI barrack_stoneCost_txt;
    //-----------------------------------

    //------------- PANELS ---------------
    [SerializeField] GameObject buildingPanel;
    [SerializeField] GameObject loggerCampPanel;
    [SerializeField] GameObject stoneMinerHutPanel;
    [SerializeField] GameObject goldMinerHutPanel; ///
    [SerializeField] GameObject barrackPanel;
    //-----------------------------------

    //------------- BUTTONS ---------------
    [SerializeField] Button openBuildingPanelButton;

    [SerializeField] Button addLoggerButton;
    [SerializeField] Button removeLoggerButton;

    [SerializeField] Button addStoneMinerButton;
    [SerializeField] Button removeStoneMinerButton;

    [SerializeField] Button addGoldMinerButton; ///
    [SerializeField] Button removeGoldMinerButton; ///

    [SerializeField] Button recruitSoldierButton;

    //-------------------------------------

    //------------- SLIDERS ----------------
    [SerializeField] Slider loggerCampSlider;
    [SerializeField] Slider stoneMinerHutSlider;
    [SerializeField] Slider goldMinerHutSlider; ///
    [SerializeField] Slider recruitmentBarSlider;
    //-------------------------------------

    private LoggerCamp loggerCamp;
    private MinerHut minerHut;
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
        minerHut = null;
        barrack = null;

        UpdateTextCostOnBuildingPanel();

        //Update unit cost (recruitment)
        soldierCostVal.text = ResourcesManager.instance.soldierPrefab.GetComponent<Unit>().costToRecruit.ToString();
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
                    _rallyFlag.position = hit.point;
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
            minerHut = _structure.GetComponent<MinerHut>();
            OpenStoneMinerHutPanel();
        }
        else if (_structure.CompareTag("goldMinerHut"))
        {
            minerHut = _structure.GetComponent<MinerHut>();
            OpenGoldMinerHutPanel();
        }
        else if (_structure.CompareTag("barrack")) //barrack
        {
            barrack = _structure.GetComponent<Barrack>();

            barrack.isSelected = true;
            barrack.transform.Find("rallyFlag").gameObject.SetActive(true);
            OpenBarrackPanel();
        }
    }

    public void DeselectBuildings()
    {
        CloseAllOpenedPanel();

        //Reset
        loggerCamp = null;
        minerHut = null;
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
        CloseGoldMinerHutPanel();
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
        if (minerHut != null)
        {
            //Update text
            stoneMinerCountTxt.text = minerHut.minerCount.ToString();
            maxStoneMinerTxt.text = minerHut.maxMiners.ToString();

            //Update health bar
            BuildingData buildingData = minerHut.GetComponent<BuildingData>();

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
        if (minerHut != null)
        {
            minerHut.AddMiner(1);
        }
        UpdateStoneMinerHutPanel();
    }
    void OnRemoveStoneMinerClick()
    {
        if (minerHut != null)
        {
            minerHut.RemoveMiner(1);
        }
        UpdateStoneMinerHutPanel();
    }

    //--------- GOLD MINER HUT ------------


    //---- PANELS 

    public void OpenGoldMinerHutPanel()
    {
        //Update button
        addGoldMinerButton.onClick.RemoveAllListeners();
        addGoldMinerButton.onClick.AddListener(OnAddGoldMinerClick);
        removeGoldMinerButton.onClick.RemoveAllListeners();
        removeGoldMinerButton.onClick.AddListener(OnRemoveGoldMinerClick);

        //Update UI
        UpdateGoldMinerHutPanel();

        //Display UI
        if (!goldMinerHutPanel.activeInHierarchy)
        {
            goldMinerHutPanel.SetActive(true);
        }
    }


    //Maj panel StoneMinerHut selon données du bâtiment sélectionné
    public void UpdateGoldMinerHutPanel()
    {
        if (minerHut != null)
        {
            //Update text
            goldMinerCountTxt.text = minerHut.minerCount.ToString();
            maxGoldMinerTxt.text = minerHut.maxMiners.ToString();

            //Update health bar
            BuildingData buildingData = minerHut.GetComponent<BuildingData>();

            if (buildingData != null)
            {
                int _maxHealth = buildingData.maxHealth;
                int _currentHealth = buildingData.currentHealth;

                stoneMinerHutSlider.maxValue = _maxHealth;
                stoneMinerHutSlider.value = _currentHealth;
            }

        }


    }

    public void CloseGoldMinerHutPanel()
    {
        goldMinerHutPanel.SetActive(false);
    }

    //---- BUTTONS

    void OnAddGoldMinerClick()
    {
        if (minerHut != null)
        {
            minerHut.AddMiner(1);
        }
        UpdateGoldMinerHutPanel();
    }
    void OnRemoveGoldMinerClick()
    {
        if (minerHut != null)
        {
            minerHut.RemoveMiner(1);
        }
        UpdateGoldMinerHutPanel();
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

        //Display panel
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

        //hide cost
        soldierCostLabel.enabled = false;
        soldierCostVal.enabled = false;

        //display
        recruitmentBarSlider.gameObject.SetActive(true);
        recruitedUnitNameTxt.enabled = true;

        //Disable recruitment button
        recruitSoldierButton.interactable = false;
    }

    public void ClearRecruitmentInfos()
    {
        //hide slider and unit name
        recruitmentBarSlider.gameObject.SetActive(false);
        recruitedUnitNameTxt.enabled = false;

        //display cost
        soldierCostLabel.enabled = true;
        soldierCostVal.enabled = true;

        UpdateRecruitmentButton();
    }

    //---- BUTTONS

    //Enable or disable recruitment button(enough population and resources)
    public void UpdateRecruitmentButton()
    {
        ResourcesManager resourcesManager = ResourcesManager.instance;
        bool isRecruitmentPossible = (resourcesManager.totalPopulation < resourcesManager.populationMax) &&
        (resourcesManager.goldBarCount >= resourcesManager.soldierPrefab.GetComponent<Unit>().costToRecruit);

        recruitSoldierButton.interactable = isRecruitmentPossible;
    }

    void OnRecruitSoldierClick()
    {
        if (barrack != null)
        {
            barrack.RecruitSoldier();
        }
    }


}
