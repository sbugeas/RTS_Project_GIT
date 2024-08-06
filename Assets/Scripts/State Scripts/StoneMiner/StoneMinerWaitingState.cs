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

        //Active & désactive objets selon état (hache et rondin)
        stoneMinerData.carriedStone.SetActive(false);
        stoneMinerData.stoneMinerPick.SetActive(false);

        //Ajout dans la liste de bûcherons inactifs
        stoneMinerHut.inactiveStoneMiners.Add(animator.transform.gameObject);

        //Détermine index de position d'attente
        int ind = stoneMinerHut.inactiveStoneMiners.Count - 1;

        //On récupère le checkpoint
        target = stoneMinerHut.transform.GetChild(ind);

        //Sécurité : si l'enfant n'existe pas, on affecte le 1er. Si il n'en a pas, on affecte la position du bâtiment
        if (target == null)
        {
            target = stoneMinerHut.transform.GetChild(0);

            if(target == null) 
            {
                target = stoneMinerHut.transform;
            }
        }

        //Déplacement à la position d'attente(target)
        agent.SetDestination(target.position);
        agent.stoppingDistance = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Si non renvoyé(isFired) et pas au camp
        if (!animator.GetBool("isFired") && !animator.GetBool("isIntoTheHut"))
        {
            //Vérifier si arrivé à destination : si oui -> s'arrêter
            if (Vector3.Distance(animator.transform.position, target.position) <= 0.5f)
            {
                animator.SetBool("isIntoTheHut", true);
                agent.SetDestination(animator.transform.position);
            }

        }


    }


}
