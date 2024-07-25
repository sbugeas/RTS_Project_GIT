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

        //Active & désactive bons objets
        loggerData.carriedLog.SetActive(true);
        loggerData.loggerAxe.SetActive(false);

        //Position entrepôt
        wareHouse = loggerData.workBuilding.GetComponent<LoggerCamp>().warehouseCheckPoint;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wareHouse != null)
        {
            agent.SetDestination(wareHouse.position);

            //Si à portée de l'entrepôt
            if (Vector3.Distance(animator.transform.position, wareHouse.position) <= depositRange)
            {
                //On arrête l'agent (test)
                agent.SetDestination(animator.transform.position);

                //On dépose le stock dans l'entrepôt
                ResourcesManager.instance.AddWood(loggerData.stock);
                loggerData.stock = 0;

                //On repasse dans l'état Idle
                animator.SetBool("isCarryingWood", false);
            }
        }        

    }
}
