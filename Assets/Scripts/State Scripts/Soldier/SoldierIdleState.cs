using UnityEngine;

//Je me suis aid� d'une source pour r�aliser ce script. Comme indiqu� dans le ReadMe, ceci est dans une d�marche d'apprentissage.
//Vous pouvez retrouver ma source dans le ReadMe

public class SoldierIdleState : StateMachineBehaviour
{
    AttackController attackController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Changement d'�tat si une cible est � proximit�
        if(attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing", true);
        }
    }


}
