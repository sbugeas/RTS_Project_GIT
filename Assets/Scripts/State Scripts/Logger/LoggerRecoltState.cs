using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoggerRecoltState : StateMachineBehaviour
{
    LoggerData loggerData;
    NavMeshAgent agent;

    float recolt_cooldown = 1.5f;
    private float timeSinceLastHit = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isGoingToRecolt", false); //test

        agent = animator.GetComponent<NavMeshAgent>();
        loggerData = animator.GetComponent<LoggerData>();

        //Active & désactive bons objets
        loggerData.carriedLog.SetActive(false);
        loggerData.loggerAxe.SetActive(true);

        animator.transform.LookAt(loggerData.targetTree);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position); //Essayer de mettre dans Enter(?)

        timeSinceLastHit += Time.deltaTime;

        if (loggerData.targetTree != null) 
        {
            //Si cooldown atteint, on coupe
            if (timeSinceLastHit >= recolt_cooldown)
            {
                animator.SetTrigger("Cut");
                timeSinceLastHit = 0.0f;
                //Rq : La méthode Recolt(loggerData) est appelé en Event de l'animation de coupe(fin)
            }
        }
        else 
        {
            if (loggerData.stock >= 0) 
            {
                animator.SetBool("isCarryingWood", true);
            }

            animator.SetBool("isRecolting", false); //test

        }

    }

}
