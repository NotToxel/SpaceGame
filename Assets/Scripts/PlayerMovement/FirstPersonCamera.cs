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
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Slider sensitivitySlider;
    float camVerticalRotation = 0f;
    public Transform player;
    
    // void Start()
    // {
    //     mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100);
    //     sensitivitySlider.value = mouseSensitivity/10;
        
    // }

    void Start() 
    {
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
        // Collect mouse inputs
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate camera around local x axis
        camVerticalRotation -= inputY;
        camVerticalRotation = Mathf.Clamp(camVerticalRotation, -clampAngle, clampAngle);
        transform.localEulerAngles = Vector3.right * camVerticalRotation;

        // Rotate player and camera around its y axis
        player.Rotate(Vector3.up * inputX);

    }

    public void AdjustSpeed()
    {
        mouseSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);   
    }

    public void LoadSensitivity() 
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("currentSensitivity");
        AdjustSpeed();
    }
}
