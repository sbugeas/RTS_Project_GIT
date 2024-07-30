using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //---------- DATA ------------
    public int populationMax = 40;
    public int totalPopulation = 10;
    public int inactivePopulationCount = 10;

    public int woodCound = 0;
    public int rockCount = 0;
    public int ironCount = 0;
    //----------------------------

    public TextMeshProUGUI woodCountTxt;
    public TextMeshProUGUI totalPopulationCountTxt;
    public TextMeshProUGUI inactivePopulationCountTxt;

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
        inactivePopulationCountTxt.text = inactivePopulationCount.ToString();
    }

    public void AddToTotalPop(int count) 
    {
        //Vérifie si on ne dépasse pas populationMax
        if (totalPopulation + count > populationMax) 
        {
            count = populationMax - totalPopulation;
        }

        totalPopulation += count;
        inactivePopulationCount += count;

        UpdatePopulation();
    }

    public void AddToInactivePop(int count)
    {
        inactivePopulationCount += count;
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

    public void RemToInactivePop(int count)
    {
        inactivePopulationCount -= count;

        if (inactivePopulationCount < 0)
        {
            inactivePopulationCount = 0;
        }
        UpdatePopulation();
    }

    
}
