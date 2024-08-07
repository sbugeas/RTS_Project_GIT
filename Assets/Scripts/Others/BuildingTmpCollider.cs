using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTmpCollider : MonoBehaviour
{
    public int collisionHit = 0;

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (!go.CompareTag("ground"))
        {
            collisionHit++;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;

        if (!go.CompareTag("ground"))
        {
            collisionHit--;
        }
    }
}
