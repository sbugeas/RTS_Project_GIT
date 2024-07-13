using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantData : MonoBehaviour
{
    public Transform workBuilding;
    public Transform targetTree;

    public GameObject carriedLog;
    public GameObject loggerAxe;

    public int stock = 0;

    [SerializeField]private int hitDamage = 40;

    public void RecoltWood() 
    {
        if(targetTree != null) 
        {
            HealthTree healthTree = targetTree.GetComponent<HealthTree>();
            int healPoints;

            healthTree.TakeDamage(hitDamage);

            //Retirer cible si plus d'HP(après hit)
            healPoints = healthTree.currentHealth;

            if (healPoints <= 0)
            {
                stock += healthTree.quantityGiven;
                targetTree = null;
            }
        }

    }
}
