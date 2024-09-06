using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerGoingToRecoltState : StateMachineBehaviour
{
    MinerData minerData;
    NavMeshAgent agent;

    public float recoltingDistance = 3.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        minerData = animator.GetComponent<MinerData>();

        //Active & désactive bons objets
        minerData.carriedResource.SetActive(false);
        minerData.minerPick.SetActive(true);

        if (minerData.targetResource != null)
        {
            agent.SetDestination(minerData.targetResource.position);
        }

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = minerData.targetResource;

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
