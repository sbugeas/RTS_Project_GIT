using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerRecoltState : StateMachineBehaviour
{
    LoggerData loggerData;
    NavMeshAgent agent;

    float recolt_cooldown = 1.5f;
    private float timeSinceLastHit = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        loggerData = animator.GetComponent<LoggerData>();

        //Active & désactive bons objets
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(true);

        animator.transform.LookAt(loggerData.targetTree);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);

        timeSinceLastHit += Time.deltaTime;

        if (loggerData.targetTree != null) 
        {
            if (timeSinceLastHit >= recolt_cooldown)
            {
                animator.SetTrigger("Cut");
                timeSinceLastHit = 0.0f;
            }
        }
        else 
        {
            if (loggerData.stock >= 0) 
            {
                animator.SetBool("isCarryingWood", true);
            }
            else animator.SetBool("isRecolting", false);

        }

    }

}
