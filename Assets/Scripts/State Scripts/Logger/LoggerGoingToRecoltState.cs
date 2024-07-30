using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerGoingToRecoltState : StateMachineBehaviour
{
    LoggerData loggerData;
    NavMeshAgent agent;

    public float recoltingDistance = 3.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        loggerData = animator.GetComponent<LoggerData>();

        //Active & désactive bons objets
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(true);

        if (loggerData.targetTree != null) 
        {
            agent.SetDestination(loggerData.targetTree.position);
        }
        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = loggerData.targetTree;

        if (target == null) 
        {
            animator.SetBool("isGoingToRecolt", false);
        }
        else 
        {
            if (Vector3.Distance(animator.transform.position, target.position) < recoltingDistance) 
            {
                animator.SetBool("isRecolting", true);
                //animator.SetBool("isGoingToRecolt", false); //test
            }
        }

    }


}
