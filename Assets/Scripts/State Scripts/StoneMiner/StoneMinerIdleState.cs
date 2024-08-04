using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerIdleState : StateMachineBehaviour
{
    StoneMinerData stoneMinerData;
    NavMeshAgent agent;

    StoneQuarry stoneQuarry;
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);
        animator.SetBool("isRecolting", false);

        agent = animator.transform.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.transform.GetComponent<StoneMinerData>();

        agent.stoppingDistance = 1;

        //Si pas de cible (ET stock nul)
        if ((stoneMinerData.targetRock == null) && (stoneMinerData.stock == 0))
        {
            stoneQuarry = stoneMinerData.workBuilding.GetComponent<StoneQuarry>();

            //On essaie d'en attribuer une
            target = stoneQuarry.GiveNearestValidRock();

            if (target == null)
            {
                animator.SetBool("isWorking", false);
            }
            else
            {
                stoneMinerData.targetRock = target;
                animator.SetBool("isGoingToRecolt", true);
            }


        }
    }
}
