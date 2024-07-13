using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class AttackController : MonoBehaviour
{
    [SerializeField] private string enemyUnitTag;

    public Transform targetToAttack;
    public Unit unitScript;
    public List<Transform> unitsInTrigger = new List<Transform>();


    private void Start()
    {
        unitScript = GetComponent<Unit>();
        targetToAttack = null;
    }

    //Peut �tre faire une liste d'unit�s qui attaque l'unit� consid�r�e, ajouter quand commande d'attaque, retirer quand commande d�placement 
    //ET on ajoute (et retire) lorsqu'une cible est donn� (et supprim�) par le script Attack Controller (Pour l'ajout, uniquement dans le cas o� !isCommandedToAttack

    private void Update()
    {
        //Si l'unit� a une cible mais que la cible n'est plus en vie (s�rement mieux � faire, + opti)
        if ((targetToAttack != null) && !(targetToAttack.gameObject.GetComponent<Unit>().isAlive)) 
        {
            targetToAttack = null;

            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            if (agent.isActiveAndEnabled) 
            { 
                agent.SetDestination(transform.position);
            }
        }
        ////

        if (targetToAttack == null && unitsInTrigger.Count > 0)
        {
            targetToAttack = FindNearestTarget();
        }
    }

    private void OnTriggerEnter(Collider other) //Modifi�, OnTriggerStay normalement
    {
        if (other.CompareTag(enemyUnitTag))
        {
            if (!unitsInTrigger.Contains(other.transform))
            {
                unitsInTrigger.Add(other.transform);
            }
        }

    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyUnitTag))
        {
            if(targetToAttack != null) 
            {
                Transform unitGone = other.transform;

                if (targetToAttack == unitGone && (unitScript.isCommandedToAttack == false))
                {
                    targetToAttack = null;
                }
            }

            if (unitsInTrigger.Contains(other.transform))
            {
                unitsInTrigger.Remove(other.transform);
            }

        }

       
    }

    private Transform FindNearestTarget() 
    {
        List<Transform> unitsInTriggerCpy = new List<Transform>();
        unitsInTriggerCpy = unitsInTrigger;

        Transform nearestUnit = null;
        float min_distance = Mathf.Infinity;

        foreach (Transform unit in unitsInTriggerCpy)
        {
            if ((unit != null) && (unit.GetComponent<Unit>().isAlive)) 
            {
                float cur_distance = Vector3.Distance(transform.position, unit.position);

                if (cur_distance < min_distance)
                {
                    min_distance = cur_distance;
                    nearestUnit = unit;
                }
            }

        }

        return nearestUnit;
    }


    //Gizmos for tests
    private void OnDrawGizmos()
    {
        /*

        //Follow Distance area
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 20f);

        //Attack Distance area
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);

        //Stop attack distance area
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawWireSphere(transform.position, 4.6f);

        */

    }

}
