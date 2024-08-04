using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerReturnHallState : StateMachineBehaviour
{
    NavMeshAgent agent;
    StoneMinerData stoneMinerData;
    Vector3 target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get component
        agent = animator.transform.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.transform.GetComponent<StoneMinerData>();

        //Update parameters
        animator.SetBool("isWorking", false);
        animator.SetBool("isIntoTheHut", false);
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);
        animator.SetBool("isCarryingStone", false);

        //Disable axe & log
        stoneMinerData.carriedStone.SetActive(false);
        stoneMinerData.stoneMinerPick.SetActive(false);

        //Immobilisation(for reset target)
        agent.SetDestination(animator.transform.position);

        //Get hall checkpoint position
        target = stoneMinerData.workBuilding.GetComponent<StoneQuarry>().hallCheckpoint.position;
        agent.stoppingDistance = 0;

        stoneMinerData.RemoveWorkBuilding();

        //Go to hall
        agent.SetDestination(target);


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If he arrived at hall
        if (Vector3.Distance(animator.transform.position, target) <= 1f)
        {
            ResourcesManager.instance.AddToTotalPop(1);
            Destroy(animator.gameObject);
        }
    }
}
