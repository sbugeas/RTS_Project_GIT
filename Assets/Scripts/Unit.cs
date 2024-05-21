using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool isCommandedToMove;

    public NavMeshAgent agent;

    //DONNEES (ownerPlayer,stats etc...)
    public int unitHealth;
    public int unitMaxHealth = 200;

    public int unitDamage = 10;
    public float attackCooldown = 1.0f;

    public Slider unitHealthSlider;

    private void Start()
    {
        unitHealth = unitMaxHealth;
        unitHealthSlider.value = unitHealth; //Peut être inutile

        agent = GetComponent<NavMeshAgent>();
        isCommandedToMove = false;

        //A MODIFIER POUR AJOUTER AUSSI A LISTE J1 ou J2 selon joueur

        //AJOUT LISTE UNITES
        UnitSelectionManager.instance.allUnitsList.Add(gameObject);

    }


    private void Update()
    {
        //L'unité a atteint sa destination
        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance) 
        {
            isCommandedToMove = false;
        }

        if (unitHealth <= 0)
        {
            DestroyThisUnit();
        }

    }

    public void TakeDamage(int _damage)
    {
        unitHealth -= _damage;

        //Faire maj slider
        unitHealthSlider.value -= _damage;
    }

    private void DestroyThisUnit() 
    {
        if (UnitSelectionManager.instance.allUnitsList.Contains(gameObject)) 
        {
            UnitSelectionManager.instance.allUnitsList.Remove(gameObject);
        }

        Destroy(this.gameObject);
    }

}
