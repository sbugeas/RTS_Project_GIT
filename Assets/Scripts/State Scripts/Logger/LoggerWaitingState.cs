using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerWaitingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    PeasantData peasantData;
    Transform target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.GetComponent<NavMeshAgent>();
        peasantData = animator.transform.GetComponent<PeasantData>();
        LoggerCamp loggerCamp = peasantData.workBuilding.GetComponent<LoggerCamp>();

        //Active & d�sactive bons objets
        peasantData.carriedLog.SetActive(false);
        peasantData.loggerAxe.SetActive(false);

        //D�termine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count;

        //Ajout � liste de b�cherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //D�termine position d'attente
        target = loggerCamp.gameObject.transform.GetChild(ind);

        agent.SetDestination(target.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    { 
        if(!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("isIntoTheCamp", true);  //Change d'�tat
        }
    }


}
