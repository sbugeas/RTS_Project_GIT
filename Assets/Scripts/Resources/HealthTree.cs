using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTree : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int currentHealth;

    public int quantityGiven = 1;
    public bool isTargeted = false;

    [SerializeField] private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    


    //Applique des d�g�ts � l'arbre et retourne le nombre de rondin(s) donn�(s) si l'arbre est d�truit, 0 sinon
    public int TakeDamage(int damage) 
    {
        //Apply damage
        if ((currentHealth - damage) < 0)
        {
            currentHealth = 0;
        }
        else 
        {
            currentHealth -= damage;
        }

        if (currentHealth == 0)
        {
            //Destruction + envoi ressources au b�cheron (via return)
            animator.SetTrigger("Cutted");
            return quantityGiven;
        }
        else return 0;
    }

    //Pour animation event
    public void DestroyThisObject() 
    {
        Destroy(this.gameObject);
    }
}
