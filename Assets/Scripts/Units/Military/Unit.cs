using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool isCommandedToMove;
    public bool isCommandedToAttack;
    public bool isAlive;

    //Pour tests
    public bool isTested = false;

    //DONNEES (ownerPlayer,stats etc...)
    public int unitHealth;
    public int unitMaxHealth = 200;
    public int unitDamage = 10;

    public float attackCooldown = 1.0f;

    public Canvas unitCanvas;
    public Slider unitHealthSlider;
    public NavMeshAgent agent;
    public Animator animator;
    public Rigidbody rb;

    AttackController attackController;


    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        unitCanvas.gameObject.SetActive(false);

        unitHealth = unitMaxHealth;
        unitHealthSlider.value = unitHealth;

        isCommandedToMove = false;
        isCommandedToAttack = false;
        isAlive = true;

        //AJOUT LISTE UNITES
        UnitSelectionManager.instance.allUnitsList.Add(gameObject);
        attackController = GetComponent<AttackController>();

    }


    private void Update()
    {
        if(agent.hasPath == false && !agent.pathPending || agent.remainingDistance <= agent.stoppingDistance) //L'unité a atteint sa destination
        {             
            isCommandedToMove = false;
            animator.SetBool("isMoving", false);
        }
        else //L'unité a un chemin
        {
            animator.SetBool("isMoving", true);
        }        

    }


    public void TakeDamage(int _damage)
    {
        if (!unitCanvas.gameObject.activeInHierarchy) 
        {
            unitCanvas.gameObject.SetActive(true);
        }

        unitHealth -= _damage;

        //Fait maj slider
        unitHealthSlider.value -= _damage;

        if (unitHealth <= 0 && isAlive == true)
        {
            KillThisUnit();
            isAlive = false;
        }
    }

    private void KillThisUnit() 
    {
        //Lancement animation de mort
        animator.SetTrigger("Dead");
        //Pour ramener dans l'état idle et y rester
        animator.SetBool("isDead", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isFollowing", false);

        if (UnitSelectionManager.instance.selectedUnitsList.Contains(gameObject))
        {
            UnitSelectionManager.instance.selectedUnitsList.Remove(gameObject);
        }
        UnitSelectionManager.instance.allUnitsList.Remove(gameObject);

        gameObject.transform.Find("indicator").gameObject.SetActive(false);

        //désactiver composants
        rb.isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        attackController.enabled = false;
        agent.enabled = false;

        unitCanvas.gameObject.SetActive(false);

        //Coroutine appelant DestroyThisUnit() au bout d'un certain temps
        StartCoroutine(WaitAndDestroy());
        
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(20f);
        Destroy(this.gameObject);
    }

    

}
