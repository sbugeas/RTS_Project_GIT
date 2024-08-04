using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRock : MonoBehaviour
{
    [SerializeField] private int maxStone = 6;
    public int remainingStone;

    public int totalHitToRecolt = 4;

    private void Start()
    {
        remainingStone = maxStone;
    }

    public void GetStone()
    {
        //Reduce stone
        remainingStone--;

        //Destruction (if no more stone)
        if (remainingStone <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
