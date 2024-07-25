using UnityEngine;
using UnityEngine.AI;

//Je me suis aid� d'une source pour r�aliser ce script. Comme indiqu� dans le ReadMe, ceci est dans une d�marche d'apprentissage.
//Vous pouvez retrouver ma source dans le ReadMe

public class SoldierAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 4.6f;

    private float timeSinceLastAttack;
    private float attckCooldwn;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();

        attckCooldwn = animator.GetComponent<Unit>().attackCooldown;
        timeSinceLastAttack = attckCooldwn;     
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Si attaque possible
        if (attackController.targetToAttack != null && animator.transform.GetComponent<Unit>().isCommandedToMove == false)
        {
            timeSinceLastAttack += Time.deltaTime;

            animator.transform.LookAt(attackController.targetToAttack);

            if (timeSinceLastAttack >= attckCooldwn)
            {
                Attack(animator);
                timeSinceLastAttack = 0.0f;

            }

            //Check si l'unit� est toujours � port�e d'attaque de la cible(et si elle n'est pas nulle)

            if (attackController.targetToAttack == null)
            {
                animator.SetBool("isFollowing", false); //Move to Idle state
                animator.SetBool("isAttacking", false);
            }
            else
            {
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

                if (distanceFromTarget > stopAttackingDistance)
                {
                    animator.SetBool("isAttacking", false); //Move to follow state
                }
            }


        }
        else 
        {
            animator.SetBool("isFollowing", false); //Move to Idle state
            animator.SetBool("isAttacking", false);
        }
        

    }

    

    private void Attack(Animator _animator) 
    {
        _animator.SetTrigger("Attack");

        //R�cup�ration d�g�t
        int unitDamage = attackController.transform.GetComponent<Unit>().unitDamage;

        //V�rification si l'ennemi a �t� tu�
        int enemyCurHealth = attackController.targetToAttack.transform.GetComponent<Unit>().unitHealth;
        bool enemyWillDie = (unitDamage >= enemyCurHealth);

        //Applique les d�gats
        attackController.targetToAttack.transform.GetComponent<Unit>().TakeDamage(unitDamage);

        if (enemyWillDie) 
        {
            _animator.GetComponent<Unit>().isCommandedToAttack = false;
            attackController.targetToAttack.gameObject.tag = "Untagged";
            attackController.targetToAttack = null;
        }
    }
   
}
