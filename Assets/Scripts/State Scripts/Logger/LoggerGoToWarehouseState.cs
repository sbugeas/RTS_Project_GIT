using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class LoggerGoToWarehouseState : StateMachineBehaviour
{
    LoggerData loggerData;
    NavMeshAgent agent;

    public float depositRange = 3.0f;

    Transform wareHouse;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        loggerData = animator.GetComponent<LoggerData>();

        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        //Active & d�sactive bons objets
        loggerData.carriedLog.SetActive(true);
        loggerData.loggerAxe.SetActive(false);

        //On r�cup�re le checkpoint de l'entrep�t et on y envoie le b�cheron
        wareHouse = loggerData.workBuilding.GetComponent<LoggerCamp>().warehouseCheckPoint;

        if(wareHouse != null) 
        {
            agent.SetDestination(wareHouse.position);
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
                ResourcesManager.instance.AddWood(loggerData.stock);
                loggerData.stock = 0;

                //On repasse dans l'�tat Idle
                animator.SetBool("isCarryingWood", false);
            }
        }        

    }
}
