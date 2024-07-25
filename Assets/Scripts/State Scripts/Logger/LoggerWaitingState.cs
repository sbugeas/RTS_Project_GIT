using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerWaitingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    LoggerData loggerData;
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.GetComponent<NavMeshAgent>();
        loggerData = animator.transform.GetComponent<LoggerData>();
        LoggerCamp loggerCamp = loggerData.workBuilding.GetComponent<LoggerCamp>();

        //Active & désactive objets selon état (hache et rondin)
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(false);

        //Détermine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count;

        //Ajout à liste de bûcherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //Détermine position d'attente
        target = loggerCamp.gameObject.transform.GetChild(ind);

        //Déplacement à la position d'attente
        agent.SetDestination(target.position);
    }


}
