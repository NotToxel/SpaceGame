// First Person Camera inspired by Unity Ace on YouTube
// https://www.youtube.com/watch?v=5Rq8A4H6Nzw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    // --- Player Camera Settings ---
    [Header("First Player Camera")]
    [SerializeField] private float clampAngle = 80f;
    public float mouseSensitivity = 2f;
    public float currentMouseSensitivity;
    float camVerticalRotation = 0f;
    public Transform player;

    void Start() 
    {
        currentMouseSensitivity = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        // Collect mouse inputs
        float inputX = Input.GetAxis("Mouse X") * currentMouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * currentMouseSensitivity;

        // Rotate camera around local x axis
        camVerticalRotation -= inputY;
        camVerticalRotation = Mathf.Clamp(camVerticalRotation, -clampAngle, clampAngle);
        transform.localEulerAngles = Vector3.right * camVerticalRotation;

        // Rotate player and camera around its y axis
        player.Rotate(Vector3.up * inputX);

    }
}