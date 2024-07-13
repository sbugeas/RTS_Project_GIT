using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerIdleState : StateMachineBehaviour
{
    PeasantData peasantData;
    NavMeshAgent agent;

    LoggerCamp loggerCamp;
    Transform target;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        agent = animator.transform.GetComponent<NavMeshAgent>();
        peasantData = animator.transform.GetComponent<PeasantData>();

        //Si pas de cible (ET stock nul)
        if ((peasantData.targetTree == null) && (peasantData.stock == 0))
        {
            loggerCamp = peasantData.workBuilding.GetComponent<LoggerCamp>();

            //On essaie d'en attribuer une
            target = loggerCamp.GiveNearestValidTree();

            if (target == null) 
            {
                animator.SetBool("isWorking", false);
            }
            else 
            {
                target.GetComponent<HealthTree>().isTargeted = true;
                peasantData.targetTree = target;
                animator.SetBool("isGoingToRecolt", true);
            }


        }
    }

}
