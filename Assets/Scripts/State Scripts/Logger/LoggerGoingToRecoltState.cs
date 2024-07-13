using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerGoingToRecoltState : StateMachineBehaviour
{
    PeasantData peasantData;
    NavMeshAgent agent;

    public float recoltingDistance = 3.0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        peasantData = animator.GetComponent<PeasantData>();

        //Active & désactive bons objets
        peasantData.carriedLog.SetActive(false);
        peasantData.loggerAxe.SetActive(true);

        if (peasantData.targetTree != null) 
        {
            agent.SetDestination(peasantData.targetTree.position);
        }
        
    }

    

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = peasantData.targetTree;

        if (target == null) 
        {
            animator.SetBool("isGoingToRecolt", false);
        }
        else 
        {
            if (Vector3.Distance(animator.transform.position, target.position) < recoltingDistance) 
            {
                animator.SetBool("isRecolting", true);
            }
        }

    }


}
