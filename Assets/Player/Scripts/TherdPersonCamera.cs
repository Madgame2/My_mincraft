using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class TherdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] float sensitivity = 3.0f; // Чувствительность мыши
    [SerializeField] float minYAngle = -20f, maxYAngle = 60f; // Ограничение вертикального угла
    [SerializeField] Camera _Camera; 
    [SerializeField] KeyCode enableRotate_key;


    private float yaw = -20.0f; // Горизонтальный угол
    private float pitch = 20.0f; // Вертикальный угол

    [SerializeField] private UnityEvent<bool> change_rotation_mode;

    public Transform cameraTransfrom
    {
        get { return _Camera.transform; }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetKey(enableRotate_key))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle); // Ограничение угла
        }

        if (Input.GetKeyDown(enableRotate_key))
        {
            change_rotation_mode?.Invoke(true);
        }
        else if (Input.GetKeyUp(enableRotate_key))
        {
            change_rotation_mode?.Invoke(false);
        }


        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;


        transform.LookAt(target.position);

    }
}
