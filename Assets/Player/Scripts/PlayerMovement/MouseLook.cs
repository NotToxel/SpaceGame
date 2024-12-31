using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float clampAngle = 80f;
    private InputManager inputManager;
    private float mouseSensitivity = 50f;
    private Vector2 mouseLook;
    private float xRotation = 0f;
    public Transform playerBody;

    void Awake()
    {
        inputManager = InputManager.Instance; 
    }

    void Update() 
    {
        mouseLook = inputManager.GetMouseDelta();
        Debug.Log(mouseLook);

        float mouseX = mouseLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // void Update()
    // {
    //     mouseLook = inputManager.GetMouseDelta();
    //     Debug.Log(mouseLook);

    //     // Collect mouse inputs
    //     float mouseX = mouseLook.x * mouseSensitivity;
    //     float mouseY = mouseLook.y * mouseSensitivity;
        
    //     // Rotate camera around local x axis
    //     camVerticalRotation -= mouseY;
    //     camVerticalRotation = Mathf.Clamp(camVerticalRotation, -clampAngle, clampAngle);
    //     transform.localEulerAngles = Vector3.right * camVerticalRotation;

    //     // Rotate player and camera around its y axis
    //     playerBody.Rotate(Vector3.up * mouseX);

    // }
}
