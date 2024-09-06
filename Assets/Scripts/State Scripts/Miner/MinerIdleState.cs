using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerIdleState : StateMachineBehaviour
{
    MinerData minerData;
    NavMeshAgent agent;

    MinerHut minerHut;
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        agent = animator.transform.GetComponent<NavMeshAgent>();
        minerData = animator.transform.GetComponent<MinerData>();

        agent.stoppingDistance = 1;

        //Si pas de cible (ET stock nul)
        if ((minerData.targetResource == null) && (minerData.stock == 0))
        {
            minerHut = minerData.workBuilding.GetComponent<MinerHut>();

            //On essaie d'en attribuer une
            target = minerHut.GiveNearestValidResource();

            if (target == null)
            {
                animator.SetBool("isWorking", false);
            }
            else
            {
                minerData.targetResource = target;
                animator.SetBool("isGoingToRecolt", true);
            }


        }
    }
}
