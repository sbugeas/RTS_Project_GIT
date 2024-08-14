using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Pour le moment, le script est fait pour ne fonctionner qu'avec le soldat -> A améliorer

public class Barrack : MonoBehaviour
{
    [SerializeField] Transform spawnPosition, rallyPosition;
    [SerializeField] GameObject soldierPrefab;

    public float timeToRecruitSoldier = 4.0f;
    public float curTime = 0.0f; 

    public bool isRecruiting;
    public bool isSelected;

    private void Start()
    {
        isRecruiting = false;
        isSelected = false;
    }

    private void Update()
    {
        if (isRecruiting) 
        {
            curTime += Time.deltaTime;

            if (curTime >= timeToRecruitSoldier) 
            {
                CompleteRecruitment();
                curTime = 0.0f;
                isRecruiting = false;
            }
        }
        
    }

    public void RecruitSoldier() 
    {
        //On débute le recrutement
        curTime = 0.0f;
        CanvasManager.instance.DisplayRecruitmentInfos();
        isRecruiting = true;
    }


    private void SpawnSoldier(Transform _target)
    {
        NavMeshHit hit;

        //Si zone naviguable proche du spawn
        if (NavMesh.SamplePosition(_target.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            //Instanciation
            GameObject newSoldier = Instantiate(soldierPrefab, hit.position, Quaternion.identity);
            Unit unitScript = newSoldier.GetComponent<Unit>();

            //Si zone naviguable proche du point de ralliement (rallyPosition)
            if (NavMesh.SamplePosition(rallyPosition.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                unitScript.isCommandedToMove = true;
                //Envoyer soldier au point de ralliement
                newSoldier.GetComponent<NavMeshAgent>().SetDestination(hit.position);
            }
            else
            {
                unitScript.isCommandedToMove = false;
                newSoldier.GetComponent<NavMeshAgent>().SetDestination(transform.position);
            }
            unitScript.enabled = true;
        }
    }



    private void CompleteRecruitment() 
    {
        SpawnSoldier(spawnPosition);

        if (isSelected)
        {
            CanvasManager.instance.ClearRecruitmentInfos();
        } 
    }

}
