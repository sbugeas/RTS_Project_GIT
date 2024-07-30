using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public int woodCost = 5;
    public int popGiven = 8;
    public int maxHealth = 100;
    public int currentHealth;

    //public float buildTime = 10.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }
}
