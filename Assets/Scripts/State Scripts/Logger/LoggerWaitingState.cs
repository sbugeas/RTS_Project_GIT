using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerWaitingState : StateMachineBehaviour
{
    Transform target;

    NavMeshAgent agent;

    LoggerData loggerData;
    LoggerCamp loggerCamp;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.GetComponent<NavMeshAgent>();
        loggerData = animator.transform.GetComponent<LoggerData>();
        loggerCamp = loggerData.workBuilding.GetComponent<LoggerCamp>();

        //Active & d�sactive objets selon �tat (hache et rondin)
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(false);

        //Ajout dans la liste de b�cherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //D�termine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count - 1;

        //On acc�de � l'enfant "checkPoints" (qui contient les emplacements d'attente)
        Transform checkPoints = loggerCamp.transform.Find("checkPoints");

        //On r�cup�re le checkpoint
        target = checkPoints.transform.GetChild(ind);

        //S�curit� : si l'enfant n'existe pas, on affecte le 1er. Si il n'en a pas, on affecte la position du b�timent
        if (target == null)
        {
            target = checkPoints.transform.GetChild(0);

            if (target == null)
            {
                target = loggerCamp.transform;
            }
        }

        //D�placement � la position d'attente(target)
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        //Si non renvoy�(isFired) et pas au camp
        if (!animator.GetBool("isFired") && !animator.GetBool("isIntoTheCamp")) 
        {
            //V�rifier si arriv� � destination : si oui -> s'arr�ter
            if (Vector3.Distance(animator.transform.position, target.position) <= 0.5f) 
            {
                animator.SetBool("isIntoTheCamp", true);
                agent.SetDestination(animator.transform.position);
            }

        }
        
        
    }

}
