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

        //Active & d�sactive objets selon �tat (hache et rondin)
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(false);

        //D�termine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count;

        //Ajout � liste de b�cherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //D�termine position d'attente
        target = loggerCamp.gameObject.transform.GetChild(ind);

        //D�placement � la position d'attente
        agent.SetDestination(target.position);
    }


}
