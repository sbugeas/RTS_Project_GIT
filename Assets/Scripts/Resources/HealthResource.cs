using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthResource : MonoBehaviour
{
    [SerializeField] private int maxResource = 6;
    public int remainingQuantity;

    public int totalHitToRecolt = 8;

    private void Start()
    {
        remainingQuantity = maxResource;
    }

    public void GetResource()
    {
        //Reduce resource
        remainingQuantity--;

        //Destruction (if no more resource)
        if (remainingQuantity <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
