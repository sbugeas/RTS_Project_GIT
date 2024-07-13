using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float rotationSmooth = 5f;

    private float _targetAngle;
    private float _currentAngle;

    private void Awake()
    {
        _currentAngle = transform.eulerAngles.y;
        _targetAngle = _currentAngle;
    }

    void Update()
    {
        GetInput();
        RotateCamera();
    }

    private void GetInput() 
    {
        if (!Input.GetMouseButton(2)) return; 
            //Calcule l'angle par rapport à la distance parcouru par la souris
            _targetAngle += Input.GetAxisRaw("Mouse X") * rotationSpeed;
    }

    private void RotateCamera() 
    {
        _currentAngle = Mathf.Lerp(_currentAngle, _targetAngle, Time.deltaTime * rotationSmooth);
        transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.up);
    }
}
