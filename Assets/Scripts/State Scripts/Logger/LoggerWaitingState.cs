using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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

        //Ajout à liste de bûcherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //Détermine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count - 1;

        //On récupère le checkpoint
        target = loggerCamp.transform.GetChild(ind);
        //Sécurité si l'enfant n'existe pas(on affecte le 1er)
        if (target == null)
        {
            target = loggerCamp.transform.GetChild(0);
        }

        //Déplacement à la position d'attente(target)
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        //Si non renvoyé et pas au camp
        if (!animator.GetBool("isFired") && !animator.GetBool("isIntoTheCamp")) 
        {
            //Vérifier si arrivé à destination : si oui -> s'arrêter
            if (Vector3.Distance(animator.transform.position, target.position) <= 0.5f) 
            {
                animator.SetBool("isIntoTheCamp", true);
                agent.SetDestination(animator.transform.position);
            }

        }
        
        
    }

}
