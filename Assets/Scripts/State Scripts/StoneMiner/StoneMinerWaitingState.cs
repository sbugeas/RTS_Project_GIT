using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneMinerWaitingState : StateMachineBehaviour
{
    Transform target;

    NavMeshAgent agent;

    StoneMinerData stoneMinerData;
    StoneMinerHut stoneMinerHut;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.GetComponent<NavMeshAgent>();
        stoneMinerData = animator.transform.GetComponent<StoneMinerData>();
        stoneMinerHut = stoneMinerData.workBuilding.GetComponent<StoneMinerHut>();

        //Active & d�sactive objets selon �tat (hache et rondin)
        stoneMinerData.carriedStone.SetActive(false);
        stoneMinerData.stoneMinerPick.SetActive(false);

        //Ajout dans la liste de b�cherons inactifs
        stoneMinerHut.inactiveStoneMiners.Add(animator.transform.gameObject);

        //D�termine index de position d'attente
        int ind = stoneMinerHut.inactiveStoneMiners.Count - 1;

        //On r�cup�re le checkpoint
        target = stoneMinerHut.transform.GetChild(ind);

        //S�curit� : si l'enfant n'existe pas, on affecte le 1er. Si il n'en a pas, on affecte la position du b�timent
        if (target == null)
        {
            target = stoneMinerHut.transform.GetChild(0);

            if(target == null) 
            {
                target = stoneMinerHut.transform;
            }
        }

        //D�placement � la position d'attente(target)
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Si non renvoy�(isFired) et pas au camp
        if (!animator.GetBool("isFired") && !animator.GetBool("isIntoTheHut"))
        {
            //V�rifier si arriv� � destination : si oui -> s'arr�ter
            if (Vector3.Distance(animator.transform.position, target.position) <= 0.5f)
            {
                animator.SetBool("isIntoTheHut", true);
                agent.SetDestination(animator.transform.position);
            }

        }


    }


}
