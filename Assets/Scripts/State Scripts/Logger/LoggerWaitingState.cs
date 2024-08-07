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

        //Active & désactive objets selon état (hache et rondin)
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(false);

        //Ajout dans la liste de bûcherons inactifs
        loggerCamp.inactiveLoggers.Add(animator.transform.gameObject);

        //Détermine index de position d'attente
        int ind = loggerCamp.inactiveLoggers.Count - 1;

        //On accède à l'enfant "checkPoints" (qui contient les emplacements d'attente)
        Transform checkPoints = loggerCamp.transform.Find("checkPoints");

        //On récupère le checkpoint
        target = checkPoints.transform.GetChild(ind);

        //Sécurité : si l'enfant n'existe pas, on affecte le 1er. Si il n'en a pas, on affecte la position du bâtiment
        if (target == null)
        {
            target = checkPoints.transform.GetChild(0);

            if (target == null)
            {
                target = loggerCamp.transform;
            }
        }

        //Déplacement à la position d'attente(target)
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        //Si non renvoyé(isFired) et pas au camp
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
