using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LoggerReturnHallState : StateMachineBehaviour
{
    NavMeshAgent agent;
    LoggerData loggerData;
    Vector3 target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get component
        agent = animator.transform.GetComponent<NavMeshAgent>();
        loggerData = animator.transform.GetComponent<LoggerData>();

        //Update parameters
        animator.SetBool("isWorking", false);
        animator.SetBool("isIntoTheCamp", false);
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);
        animator.SetBool("isCarryingWood", false);

        //Disable axe & log
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(false);

        //Immobilisation(for reset target)
        agent.SetDestination(animator.transform.position);

        //Get hall checkpoint position
        target = loggerData.workBuilding.GetComponent<LoggerCamp>().hallCheckpoint.position;
        agent.stoppingDistance = 0;

        loggerData.RemoveWorkBuilding();

        //Go to hall
        agent.SetDestination(target);

        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If he arrived at hall
        if (Vector3.Distance(animator.transform.position, target) <= 1f)
        {
            ResourcesManager.instance.AddToInactivePop(1);
            Destroy(animator.gameObject);
        }
    }

}
