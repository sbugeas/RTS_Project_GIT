using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerGoingToRecoltState : StateMachineBehaviour
{
    StoneMinerData stoneMinerData;
    NavMeshAgent agent;

    public float recoltingDistance = 3.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.GetComponent<StoneMinerData>();

        //Active & désactive bons objets
        stoneMinerData.carriedStone.SetActive(false);
        stoneMinerData.stoneMinerPick.SetActive(true);

        if (stoneMinerData.targetRock != null)
        {
            agent.SetDestination(stoneMinerData.targetRock.position);
        }

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = stoneMinerData.targetRock;

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
