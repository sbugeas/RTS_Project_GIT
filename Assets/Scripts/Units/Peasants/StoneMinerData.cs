using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMinerData : MonoBehaviour
{
    public Transform workBuilding;
    public Transform targetRock;

    public GameObject carriedStone;
    public GameObject stoneMinerPick;

    public int stock = 0;

    private int hitsCount = 0; //Incrémenté et réinitialisée dans méthode "CheckIfHitCountIsValid"

    public void RecoltStone()
    {
        if (targetRock != null)
        {
            HealthRock healthRock = targetRock.GetComponent<HealthRock>();
            int remainingStone = healthRock.remainingStone;

            if (remainingStone > 0) 
            {
                stock++;
                healthRock.GetStone();
                targetRock = null;
            }
        }
    }


    public void RemoveWorkBuilding()
    {
        workBuilding = null;
    }

    
    public void CheckHitsCount() 
    {
        hitsCount++;

        if (targetRock != null && (hitsCount >= targetRock.GetComponent<HealthRock>().totalHitToRecolt)) 
        {
            RecoltStone();
            hitsCount = 0;
        }
    }
    


}
