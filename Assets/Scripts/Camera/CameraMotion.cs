using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Je me suis aidé d'une source pour réaliser ce script. Comme indiqué dans le ReadMe, ceci est dans une démarche d'apprentissage.
//Vous pouvez retrouver ma source dans le ReadMe

public class CameraMotion : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float _smoothing = 5f;
    [SerializeField] private Vector2 _range = new Vector2(100, 100);

    private Vector3 targetPosition;
    private Vector3 cameraMovement;

    private void Awake()
    {
        targetPosition = transform.position;
    }


    void Update()
    {
        GetInput();
        MoveCamera();
    }



    private bool IsInBounds(Vector3 _position)
    {
        return _position.x > -(_range.x) &&
               _position.x < _range.x &&
               _position.z > -(_range.y) &&
               _position.z < _range.y;
    }

    private void GetInput()
    {
        //Inputs
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 _right = transform.right * x;
        Vector3 _forward = transform.forward * z;

        cameraMovement = (_right + _forward).normalized;
    }

    private void MoveCamera()
    {
        Vector3 nextTargetPosition = targetPosition + cameraMovement * speed;

        if (IsInBounds(nextTargetPosition)) 
        {
            targetPosition = nextTargetPosition;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _smoothing);
    }

    
}
