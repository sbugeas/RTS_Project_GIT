using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;

    public float attackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        agent = animator.GetComponent<NavMeshAgent>();
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Transition to Idle State (no target)
        if(attackController.targetToAttack == null) 
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {
            //Si l'unité n'a pas reçu une commande de déplacement
            if (animator.transform.GetComponent<Unit>().isCommandedToMove == false) 
            {
                //Moving unit toward target
                agent.SetDestination(attackController.targetToAttack.position);
                //Orientation for animations (Maybe useless)

                //Test, à remettre
                //animator.transform.LookAt(attackController.targetToAttack);


                //Transition to Attack State(if unit can attack depending on attackingDistance)
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

                if(distanceFromTarget < attackingDistance) 
                {
                    //Stop unit when he reach target
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);
                }

            }


        }


        

    }


}
