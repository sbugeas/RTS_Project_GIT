using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinerWaitingState : StateMachineBehaviour
{
    Transform target;

    NavMeshAgent agent;

    MinerData minerData;
    MinerHut minerHut;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.transform.GetComponent<NavMeshAgent>();
        minerData = animator.transform.GetComponent<MinerData>();
        minerHut = minerData.workBuilding.GetComponent<MinerHut>();

        //Active & d�sactive objets selon �tat (hache et rondin)
        minerData.carriedResource.SetActive(false);
        minerData.minerPick.SetActive(false);

        //Ajout dans la liste de mineurs inactifs
        minerHut.inactiveMiners.Add(animator.transform.gameObject);

        //D�termine index de position d'attente
        int ind = minerHut.inactiveMiners.Count - 1;

        //On acc�de � l'enfant "checkPoints" (qui contient les emplacements d'attente)
        Transform checkPoints = minerHut.transform.Find("checkPoints");

        //On r�cup�re le checkpoint
        target = checkPoints.transform.GetChild(ind);

        //S�curit� : si l'enfant n'existe pas, on affecte le 1er. Si il n'en a pas, on affecte la position du b�timent
        if (target == null)
        {
            target = checkPoints.transform.GetChild(0);

            if(target == null) 
            {
                target = minerHut.transform;
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
