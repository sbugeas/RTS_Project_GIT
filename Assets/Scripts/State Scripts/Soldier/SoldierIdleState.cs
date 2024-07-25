using UnityEngine;

//Je me suis aidé d'une source pour réaliser ce script. Comme indiqué dans le ReadMe, ceci est dans une démarche d'apprentissage.
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
        //Changement d'état si une cible est à proximité
        if(attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing", true);
        }
    }


}
