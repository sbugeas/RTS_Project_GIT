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
    public int rockCount = 0;
    public int gold = 0;
    //----------------------------

    public TextMeshProUGUI woodCountTxt;
    public TextMeshProUGUI totalPopulationCountTxt;
    public TextMeshProUGUI maxPopulationCountTxt;

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
        UpdatePopulation();
        UpdateWood();

    }

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

    //Maj canvas
    private void UpdateWood() 
    {
        woodCountTxt.text = woodCound.ToString();
    }

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

    
}
