using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 25f;
    [SerializeField] private float zoomSmooth = 5f;
    [SerializeField] private Vector2 _range = new Vector2(30f, 70f);
    [SerializeField] private Transform _cameraHolder;

    //On récupère la direction de l'axe Z du CameraHolder
    private Vector3 _cameraDirection => transform.InverseTransformDirection(_cameraHolder.forward);
    private Vector3 targetPosition;
    private float _input;

    void Awake()
    {
        targetPosition = _cameraHolder.localPosition;
    }

    void Update()
    {
        GetInput();
        Zoom();
    }

    private void GetInput() 
    {
        //1 avant, -1 arrière, 0 rien
        _input = Input.GetAxisRaw("Mouse ScrollWheel");
    }

    private bool isInBounds(Vector3 _position) 
    { 
        return _position.magnitude > _range.x && _position.magnitude < _range.y;
    }

    private void Zoom()
    {
        Vector3 nextTargetPosition = targetPosition + _cameraDirection * (_input * zoomSpeed);

        if (isInBounds(nextTargetPosition))
        {
            targetPosition = nextTargetPosition;
        }

        _cameraHolder.localPosition = Vector3.Lerp(_cameraHolder.localPosition, targetPosition, Time.deltaTime * zoomSmooth);
    }
}
