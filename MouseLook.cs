using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10;
    public Transform target;
    public float distFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40,85);

    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    
    float yaw;
    float pitch;

    public ThirdPersonCamera scriptReference2;
    public MouseLook firstPersonCamera2;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        scriptReference2 = GetComponent<ThirdPersonCamera>();
        firstPersonCamera2 = GetComponent<MouseLook>();
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position;

        if (Input.GetKey("t"))
        {
            firstPersonCamera2.enabled = false;
            scriptReference2.enabled = true;
        }
    }
}
