using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
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

    public ThirdPersonCamera scriptReference;
    public MouseLook firstPersonCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        scriptReference = GetComponent<ThirdPersonCamera>();
        scriptReference.enabled = true;
        firstPersonCamera = GetComponent<MouseLook>();
        firstPersonCamera.enabled = false;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distFromTarget;

        if (Input.GetKey("f"))
        {
            scriptReference.enabled = false;
            firstPersonCamera.enabled = true;
        }
    }
}
