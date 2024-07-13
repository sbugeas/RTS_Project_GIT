using UnityEngine;
using UnityEngine.AI;

public class SoldierAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 4.6f;

    private float timeSinceLastAttack;
    private float attckCooldwn;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();

        attckCooldwn = animator.GetComponent<Unit>().attackCooldown;
        timeSinceLastAttack = attckCooldwn;     
    }



    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
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
        //Animation attaque
        _animator.SetTrigger("Attack");

        //Get unit damage
        int unitDamage = attackController.transform.GetComponent<Unit>().unitDamage;

        //Check if enemy will die
        int enemyCurHealth = attackController.targetToAttack.transform.GetComponent<Unit>().unitHealth;
        bool enemyWillDie = (unitDamage >= enemyCurHealth);

        //Call TakeDamage() method on the enemy unit
        attackController.targetToAttack.transform.GetComponent<Unit>().TakeDamage(unitDamage);

        if (enemyWillDie) 
        {
            _animator.GetComponent<Unit>().isCommandedToAttack = false;
            attackController.targetToAttack.gameObject.tag = "Untagged";
            attackController.targetToAttack = null;
        }
    }
   
}
