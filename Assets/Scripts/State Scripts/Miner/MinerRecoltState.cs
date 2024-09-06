using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerRecoltState : StateMachineBehaviour
{
    MinerData minerData;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    float recolt_cooldown = 1.5f;
    private float timeSinceLastHit = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false);

        agent = animator.GetComponent<NavMeshAgent>();
        obstacle = animator.GetComponent<NavMeshObstacle>();

        minerData = animator.GetComponent<MinerData>();

        //Active & désactive bons objets
        minerData.carriedResource.SetActive(false);
        minerData.minerPick.SetActive(true);

        agent.SetDestination(animator.transform.position);
        animator.transform.LookAt(minerData.targetResource);

        //Immobilisation
        agent.enabled = false;
        obstacle.enabled = true;


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceLastHit += Time.deltaTime;

        if (minerData.targetResource != null)
        {
            //On regarde le rocher
            animator.transform.LookAt(minerData.targetResource);

            //Si cooldown atteint, on mine
            if (timeSinceLastHit >= recolt_cooldown)
            {
                animator.SetTrigger("Mine");
                //Rq: CheckIfHitCountIsValid() est appelée à la fin de l'animation de minage

                timeSinceLastHit = 0;
            }
        }
        else //Plus de cible
        {
            //Fin de l'immobilisation
            obstacle.enabled = false;
            agent.enabled = true;

            //Si stock > 0 et n'a pas été renvoyé entre temps
            if (minerData.stock > 0 && !animator.GetBool("isFired"))
            {
                animator.SetBool("isCarryingResource", true);
            }

            animator.SetBool("isRecolting", false);

        }

    }

}
