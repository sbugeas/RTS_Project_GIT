using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerIdleState : StateMachineBehaviour
{
    LoggerData loggerData;
    NavMeshAgent agent;

    LoggerCamp loggerCamp;
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        agent = animator.transform.GetComponent<NavMeshAgent>();
        loggerData = animator.transform.GetComponent<LoggerData>();

        agent.stoppingDistance = 1;

        //Si pas de cible (ET stock nul)
        if ((loggerData.targetTree == null) && (loggerData.stock == 0))
        {
            loggerCamp = loggerData.workBuilding.GetComponent<LoggerCamp>();

            //On essaie d'en attribuer une
            target = loggerCamp.GiveNearestValidTree();

            if (target == null) 
            {
                animator.SetBool("isWorking", false);
            }
            else 
            {
                target.GetComponent<HealthTree>().isTargeted = true;
                loggerData.targetTree = target;
                animator.SetBool("isGoingToRecolt", true);
            }


        }
    }

}
