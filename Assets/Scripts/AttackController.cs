using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    public Unit unitScript;

    private void Start()
    {
        unitScript = GetComponent<Unit>();
        targetToAttack = null;
    }

    private void OnTriggerStay(Collider other)
    {
        //Si ennemi à proximité et pas de cible
        if (other.CompareTag("enemy") && (targetToAttack == null))
        {
            targetToAttack = other.transform;
        }
        
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy") && (targetToAttack != null))
        {
            Transform unitGone = other.transform;

            if (targetToAttack == unitGone) 
            {
                targetToAttack = null;
            }
        }
    }
    
    
    
    //Gizmos for tests
    private void OnDrawGizmos()
    {
        //Follow Distance area
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f);

        //Attack Distance area
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        //Stop attack distance area
        Gizmos.color = UnityEngine.Color.blue;
        Gizmos.DrawWireSphere(transform.position, 3.2f);

    }
}
