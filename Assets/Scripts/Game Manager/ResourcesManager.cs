using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //---------- DATA ------------
    public int population_limit = 100;
    public int populationMax = 10;
    public int totalPopulation = 0;

    public int woodCound = 0;
    public int stoneCount = 0;
    public int goldOreCount = 0;
    public int goldBarCount = 0;
    //----------------------------

    public TextMeshProUGUI woodCountTxt;
    public TextMeshProUGUI stoneCountTxt;
    public TextMeshProUGUI goldOreCountTxt;
    public TextMeshProUGUI goldBarCountTxt;

    public TextMeshProUGUI totalPopulationCountTxt;
    public TextMeshProUGUI maxPopulationCountTxt;

    //Pour accès aux données(ex : cost)
    public GameObject soldierPrefab; //test

    public static ResourcesManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Il y a plus d'une instance de ResourcesManager dans la scène !");
            return;
        }

        instance = this;
    }

    public void Start()
    {
        //Initialisation UI
        UpdateResourcesPanel();

    }

    //test (pas d'Update initialement // à retirer)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddGoldBar(1);
        }
    }
    //test

    public void UpdateResourcesPanel() 
    {
        UpdatePopulation();
        UpdateWood();
        UpdateStone();
        UpdateGoldOre();
        UpdateGoldBar();
    }

    //----------------------
    public void AddWood(int count) 
    {
        woodCound += count;

        UpdateWood();
    }

    public void RemoveWood(int count)
    {
        woodCound -= count;

        if (woodCound < 0)
        {
            woodCound = 0;
        }

        UpdateWood();

    }

    private void UpdateWood() 
    {
        woodCountTxt.text = woodCound.ToString();
    }
    //----------------------

    //----------------------
    public void AddStone(int count)
    {
        stoneCount += count;

        UpdateStone();
    }

    public void RemoveStone(int count)
    {
        stoneCount -= count;

        if (stoneCount < 0)
        {
            stoneCount = 0;
        }

        UpdateStone();

    }

    private void UpdateStone() 
    {
        stoneCountTxt.text = stoneCount.ToString();
    }
    //----------------------

    //----------------------
    public void AddGoldOre(int count)
    {
        goldOreCount += count;

        UpdateGoldOre();
    }

    public void RemoveGoldOre(int count)
    {
        goldOreCount -= count;

        if (goldOreCount < 0)
        {
            goldOreCount = 0;
        }

        UpdateGoldOre();

    }

    private void UpdateGoldOre()
    {
        goldOreCountTxt.text = goldOreCount.ToString();
    }
    //----------------------

    //----------------------
    public void AddGoldBar(int count)
    {
        goldBarCount += count;

        UpdateGoldBar();
    }

    public void RemoveGoldBar(int count)
    {
        goldBarCount -= count;

        if (goldBarCount < 0)
        {
            goldBarCount = 0;
        }

        UpdateGoldBar();

    }

    private void UpdateGoldBar()
    {
        goldBarCountTxt.text = goldBarCount.ToString();

        //Update recruitement button (if enough goldBar) //test
        CanvasManager.instance.UpdateRecruitmentButton();
    }
    //----------------------

    //----------------------
    private void UpdatePopulation() 
    {
        totalPopulationCountTxt.text = totalPopulation.ToString();
        maxPopulationCountTxt.text = populationMax.ToString();
    }

    public void AddToTotalPop(int count) 
    {
        //Vérifie si on ne dépasse pas populationMax
        if (totalPopulation + count > populationMax) 
        {
            count = populationMax - totalPopulation;
        }

        totalPopulation += count;

        UpdatePopulation();
    }

    public void AddToMaxPop(int count)
    {
        populationMax += count;

        if (populationMax > population_limit) 
        {
            populationMax = population_limit;
        }

        UpdatePopulation();
    }

    public void RemToTotalPop(int count)
    {
        totalPopulation -= count;

        if (totalPopulation < 0) 
        {
            totalPopulation = 0;
        }
        UpdatePopulation();
    }

    public void RemToMaxPop(int count)
    {
        populationMax -= count;

        if (populationMax < 0)
        {
            populationMax = 0;
        }
        UpdatePopulation();
    }
    //----------------------


}
