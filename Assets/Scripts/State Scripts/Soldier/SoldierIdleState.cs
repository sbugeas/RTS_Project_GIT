using UnityEngine;

public class SoldierIdleState : StateMachineBehaviour
{

    AttackController attackController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Changement d'�tat si une cible est � proximit�
        if(attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing", true);
        }
    }


}
