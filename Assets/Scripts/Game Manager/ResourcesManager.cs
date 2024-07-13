using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //---------- DATA ------------
    //public int limitOfPopulation = 40;
    public int totalPopulation = 10;
    public int inactivePopulationCount = 10; //Dans l'idée, 3 construisent donc actifs(sera fait + tard)

    public int woodCound = 0;
    public int rockCount = 0;
    public int ironCount = 0;
    //----------------------------

    public TextMeshProUGUI woodCountTxt;
    public TextMeshProUGUI totalPopulationCountTxt;
    public TextMeshProUGUI inactivePopulationCountTxt;

    public static ResourcesManager instance;

    public void Awake()
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
        totalPopulationCountTxt.text = totalPopulation.ToString();
        inactivePopulationCountTxt.text = inactivePopulationCount.ToString();

    }

    public void AddWood(int count) 
    {
        woodCound += count;

        //Maj canvas
        woodCountTxt.text = woodCound.ToString();
    }

    public void RemoveWood(int count)
    {
        woodCound -= count;

        if (woodCound < 0)
        {
            woodCound = 0;
        }

        //Maj canvas
        woodCountTxt.text = woodCound.ToString();
    }

    public void AddToTotalPop(int count) 
    {
        totalPopulation += count;
        totalPopulationCountTxt.text = totalPopulation.ToString();
    }

    public void AddToInactivePop(int count)
    {
        inactivePopulationCount += count;
        inactivePopulationCountTxt.text = inactivePopulationCount.ToString();
    }

    public void RemToTotalPop(int count)
    {
        totalPopulation -= count;

        if (totalPopulation < 0) 
        {
            totalPopulation = 0;
        } 
        totalPopulationCountTxt.text = totalPopulation.ToString();
    }

    public void RemToInactivePop(int count)
    {
        inactivePopulationCount -= count;

        if (inactivePopulationCount < 0)
        {
            inactivePopulationCount = 0;
        }
        inactivePopulationCountTxt.text = inactivePopulationCount.ToString();
    }


}
