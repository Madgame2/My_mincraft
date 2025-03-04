using System;
using UnityEngine;
using UnityEngine.Events;

public class TherdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] float sensitivity = 3.0f; // ���������������� ����
    [SerializeField] float minYAngle = -20f, maxYAngle = 60f; // ����������� ������������� ����
    [SerializeField] Camera _Camera; 
    [SerializeField] KeyCode enableRotate_key;

    [SerializeField] public  UnityEvent<Transform> Camera_transform; 

    private float yaw = -20.0f; // �������������� ����
    private float pitch = 20.0f; // ������������ ����



    private void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetKey(enableRotate_key))
        {
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle); // ����������� ����

            Camera_transform?.Invoke(_Camera.transform);
        }


        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;


        transform.LookAt(target.position);
    }
}
