using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerData : MonoBehaviour
{
    public Transform workBuilding;
    public Transform targetResource;

    public GameObject carriedResource;
    public GameObject minerPick;

    public int stock = 0;

    private int hitsCount = 0; //Incr�ment� et r�initialis�e dans m�thode "CheckIfHitCountIsValid"

    public void Recolt()
    {
        if (targetResource != null)
        {
            HealthResource healthResource = targetResource.GetComponent<HealthResource>();
            int remainingQuantity = healthResource.remainingQuantity;

            
            if (remainingQuantity > 0) 
            {
                stock++;
                healthResource.GetResource();
                targetResource = null;
            }
            
        }
    }


    public void RemoveWorkBuilding()
    {
        workBuilding = null;
    }

    
    //Appell�e � la fin de l'animation de minage
    public void CheckHitsCount() 
    {
        hitsCount++;

        if (targetResource != null && (hitsCount >= targetResource.GetComponent<HealthResource>().totalHitToRecolt)) 
        {
            Recolt();
            hitsCount = 0;
        }
    }
    


}
