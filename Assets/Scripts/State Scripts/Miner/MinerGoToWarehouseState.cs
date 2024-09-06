using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MinerGoToWarehouseState : StateMachineBehaviour
{
    MinerData minerData;
    NavMeshAgent agent;

    private float initialSpeed;
    private float depositRange = 3.0f;

    Transform wareHouse;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        minerData = animator.GetComponent<MinerData>();

        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        //Active & désactive bons objets
        minerData.carriedResource.SetActive(true);
        minerData.minerPick.SetActive(false);

        //Réduction vitesse d'1/3 (+ save vitesse initiale)
        initialSpeed = agent.speed;
        agent.speed = (initialSpeed / 1.5f);

        //On récupère le checkpoint de l'entrepôt et on y envoie le mineur
        wareHouse = minerData.workBuilding.GetComponent<MinerHut>().warehouseCheckPoint;

        if (wareHouse != null)
        {
            agent.SetDestination(wareHouse.position);
        }
        else
        {
            animator.SetBool("isWorking", false);
            animator.SetBool("isCarryingResource", false);
        }

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wareHouse != null && !animator.GetBool("isFired"))
        {
            //Si à portée de l'entrepôt
            if (Vector3.Distance(animator.transform.position, wareHouse.position) <= depositRange)
            {
                //Immobilisation
                agent.SetDestination(animator.transform.position);

                //On dépose le stock dans l'entrepôt (avant, on vérifie le type de mineur via son tag)
                string _tag = animator.transform.tag;

                if (_tag == "stoneMiner")
                {
                    ResourcesManager.instance.AddStone(minerData.stock);
                }
                else if (_tag == "goldMiner")
                {
                    ResourcesManager.instance.AddGoldOre(minerData.stock);
                }
               
                minerData.stock = 0;

                //On repasse dans l'état Idle
                animator.SetBool("isCarryingResource", false);
            }
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = initialSpeed;
    }
}
