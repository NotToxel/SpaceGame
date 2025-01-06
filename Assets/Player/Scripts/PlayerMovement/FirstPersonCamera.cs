// First Person Camera inspired by Unity Ace on YouTube
// https://www.youtube.com/watch?v=5Rq8A4H6Nzw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCamera : MonoBehaviour
{
    // --- Player Camera Settings ---
    [Header("First Player Camera")]
    [SerializeField] private float clampAngle = 80f;
    public float savedSensitivity = 2f;
    public float mouseSensitivity = 1f;
    public float currentMouseSensitivity;
    float camVerticalRotation = 0f;
    public Transform player;
    private bool camEnabled;

    // --- Sensitivity Slider Settings ---
    [Header("Sensitivity Slider")]
    [SerializeField] private Slider sensitivitySlider;

    void Awake() {
        sensitivitySlider.value = savedSensitivity;
    }

    void Start() 
    {
        camEnabled = true;
        if (PlayerPrefs.HasKey("currentSensitivity"))
        {
            LoadSensitivity();
        }
        else
        {
            AdjustSpeed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camEnabled == true) {
            currentMouseSensitivity = mouseSensitivity;

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

    // Adjusts players sensitivity
    public void AdjustSpeed()
    {
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);   
    }

    // Loads players sensitivity
    public void LoadSensitivity() 
    {
        sensitivitySlider.value = mouseSensitivity;
        AdjustSpeed();
    }

    public void EnableCam() {
        camEnabled = true;
    }

    public void DisableCam() {
        camEnabled = false;
    }
}
