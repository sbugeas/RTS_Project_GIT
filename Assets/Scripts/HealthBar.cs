using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform targetUnit; // Référence à l'unité associée au Slider


    // Update is called once per frame
    void Update()
    {
        if(targetUnit != null) 
        {
            // Mettre à jour position du Slider pour qu'il suive l'unité
            Vector3 targetPosition = targetUnit.position + (Vector3.up * 6.0f) + (Vector3.right * 0.5f);
            transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }
    }

}
