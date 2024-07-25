using UnityEngine;
using UnityEngine.AI;

//Je me suis aid� d'une source pour r�aliser ce script. Comme indiqu� dans le ReadMe, ceci est dans une d�marche d'apprentissage.
//Vous pouvez retrouver ma source dans le ReadMe
public class SoldierFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;

    public float attackingDistance = 4f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        agent = animator.GetComponent<NavMeshAgent>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Transition to Idle State (no target)
        if(attackController.targetToAttack == null) 
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {
            //Si l'unit� n'a pas re�u une commande de d�placement
            if (animator.transform.GetComponent<Unit>().isCommandedToMove == false) 
            {
                if (agent.isActiveAndEnabled)
                {
                    agent.SetDestination(attackController.targetToAttack.position);
                }

                //Transition to Attack State(if unit can attack depending on attackingDistance)
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

                //Stop unit when he reach target
                if (distanceFromTarget < attackingDistance) 
                {
                    if (agent.isActiveAndEnabled)
                    {
                        agent.SetDestination(animator.transform.position);
                    }
                    
                    animator.SetBool("isAttacking", true);
                }

            }


        }


        

    }


}
