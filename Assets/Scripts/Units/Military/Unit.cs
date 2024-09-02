using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour
{
    public bool isCommandedToMove;
    public bool isCommandedToAttack;
    public bool isAlive;

    public bool nearOfDestination;
    public bool hasOrCalculatePath;

    public int unitHealth;
    public int unitMaxHealth = 200;
    public int unitDamage = 10;
    public float attackCooldown = 1.0f;
    public int costToRecruit = 1; //test

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

        isAlive = true;
        isCommandedToAttack = false;


        //AJOUT LISTE UNITES
        UnitSelectionManager.instance.allUnitsList.Add(gameObject);
        attackController = GetComponent<AttackController>();

    }


    private void Update()
    {
        nearOfDestination = agent.hasPath && (agent.remainingDistance <= agent.stoppingDistance);
        hasOrCalculatePath = agent.hasPath || agent.pathPending;

        //L'unité n'a pas de destination et n'en calcule pas
        if (!hasOrCalculatePath)
        {
            //Joue animation d'attente
            animator.SetBool("isMoving", false);       
        }
        else //L'unité a une destination
        {
            //Joue animation de course
            animator.SetBool("isMoving", true);
        }

        //Si l'unité est arrivée à destination
        if (nearOfDestination)
        {
            //Immobilisation
            agent.ResetPath();
            isCommandedToMove = false;
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
