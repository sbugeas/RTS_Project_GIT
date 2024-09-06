using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerReturnHallState : StateMachineBehaviour
{
    NavMeshAgent agent;
    MinerData minerData;
    Vector3 target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get component
        agent = animator.transform.GetComponent<NavMeshAgent>();
        minerData = animator.transform.GetComponent<MinerData>();

        //Update parameters
        animator.SetBool("isWorking", false);
        animator.SetBool("isIntoTheHut", false);
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);
        animator.SetBool("isCarryingResource", false);

        //Disable axe & log
        minerData.carriedResource.SetActive(false);
        minerData.minerPick.SetActive(false);

        //Immobilisation(for reset target)
        agent.SetDestination(animator.transform.position);

        //Get hall checkpoint position
        target = minerData.workBuilding.GetComponent<MinerHut>().hallCheckpoint.position;
        agent.stoppingDistance = 0;

        minerData.RemoveWorkBuilding();

        //Go to hall
        agent.SetDestination(target);


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If he arrived at hall
        if (Vector3.Distance(animator.transform.position, target) <= 1f)
        {
            ResourcesManager.instance.RemToTotalPop(1);
            Destroy(animator.gameObject);
        }
    }
}
