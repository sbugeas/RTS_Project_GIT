using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerGoToWarehouseState : StateMachineBehaviour
{
    StoneMinerData stoneMinerData;
    NavMeshAgent agent;

    private float initialSpeed;
    private float depositRange = 3.0f;

    Transform wareHouse;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.GetComponent<StoneMinerData>();

        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        //Active & d�sactive bons objets
        stoneMinerData.carriedStone.SetActive(true);
        stoneMinerData.stoneMinerPick.SetActive(false);

        //R�duction vitesse d'1/3 (+ save vitesse initiale)
        initialSpeed = agent.speed;
        agent.speed = (initialSpeed / 2);

        //On r�cup�re le checkpoint de l'entrep�t et on y envoie le mineur
        wareHouse = stoneMinerData.workBuilding.GetComponent<StoneMinerHut>().warehouseCheckPoint;

        if (wareHouse != null)
        {
            agent.SetDestination(wareHouse.position);
        }
        else
        {
            animator.SetBool("isWorking", false);
            animator.SetBool("isCarryingStone", false);
        }

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wareHouse != null && !animator.GetBool("isFired"))
        {
            //Si � port�e de l'entrep�t
            if (Vector3.Distance(animator.transform.position, wareHouse.position) <= depositRange)
            {
                agent.SetDestination(animator.transform.position);

                //On d�pose le stock dans l'entrep�t
                ResourcesManager.instance.AddStone(stoneMinerData.stock);
                stoneMinerData.stock = 0;

                //On repasse dans l'�tat Idle
                animator.SetBool("isCarryingStone", false);
            }
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.speed = initialSpeed;
    }
}
