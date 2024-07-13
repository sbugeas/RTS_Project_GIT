using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerRecoltState : StateMachineBehaviour
{
    PeasantData peasantData;
    NavMeshAgent agent;

    float recolt_cooldown = 1.5f;
    private float timeSinceLastHit = 0.0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        peasantData = animator.GetComponent<PeasantData>();

        //Active & désactive bons objets
        peasantData.carriedLog.SetActive(false);
        peasantData.loggerAxe.SetActive(true);

        animator.transform.LookAt(peasantData.targetTree);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);

        timeSinceLastHit += Time.deltaTime;

        if (peasantData.targetTree != null) 
        {
            if (timeSinceLastHit >= recolt_cooldown)
            {
                animator.SetTrigger("Cut");
                timeSinceLastHit = 0.0f;
            }
        }
        else 
        {
            if (peasantData.stock >= 0) 
            {
                animator.SetBool("isCarryingWood", true);
            }
            else animator.SetBool("isRecolting", false);

        }

    }

}
