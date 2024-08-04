using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerRecoltState : StateMachineBehaviour
{
    StoneMinerData stoneMinerData;
    NavMeshAgent agent;

    float recolt_cooldown = 1.5f;
    private float timeSinceLastHit = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);

        agent = animator.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.GetComponent<StoneMinerData>();

        //Active & désactive bons objets
        stoneMinerData.carriedStone.SetActive(false);
        stoneMinerData.stoneMinerPick.SetActive(true);

        agent.SetDestination(animator.transform.position);
        animator.transform.LookAt(stoneMinerData.targetRock);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceLastHit += Time.deltaTime;

        if (stoneMinerData.targetRock != null)
        {
            //Si cooldown atteint, on mine
            if (timeSinceLastHit >= recolt_cooldown)
            {
                animator.SetTrigger("Mine");
                //Rq: CheckIfHitCountIsValid() est appelée à la fin de l'animation de minage

                timeSinceLastHit = 0;
            }
        }
        else
        {
            //Si stock > 0 et n'a pas été renvoyé entre temps
            if (stoneMinerData.stock >= 0 && !animator.GetBool("isFired"))
            {
                animator.SetBool("isCarryingStone", true);
            }

            animator.SetBool("isRecolting", false);

        }

    }
}
