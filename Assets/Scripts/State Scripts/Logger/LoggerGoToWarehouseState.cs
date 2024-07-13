using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class LoggerGoToWarehouseState : StateMachineBehaviour
{
    PeasantData peasantData;
    NavMeshAgent agent;

    public float depositRange = 3.0f;

    Transform wareHouse;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        peasantData = animator.GetComponent<PeasantData>();

        //Active & d�sactive bons objets
        peasantData.carriedLog.SetActive(true);
        peasantData.loggerAxe.SetActive(false);

        //Position entrep�t
        wareHouse = peasantData.workBuilding.GetComponent<LoggerCamp>().warehouseCheckPoint;
    }



    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wareHouse != null)
        {
            agent.SetDestination(wareHouse.position);

            //Si � port�e de l'entrep�t
            if (Vector3.Distance(animator.transform.position, wareHouse.position) <= depositRange)
            {
                //On arr�te l'agent (test)
                agent.SetDestination(animator.transform.position);

                //On d�pose le stock dans l'entrep�t
                ResourcesManager.instance.AddWood(peasantData.stock);
                peasantData.stock = 0;

                //On repasse dans l'�tat Idle
                animator.SetBool("isCarryingWood", false);
            }
        }        

    }
}
