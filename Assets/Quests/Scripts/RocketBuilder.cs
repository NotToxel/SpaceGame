using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBuilder : MonoBehaviour
{
    public GameObject[] rocketStages; // Assign rocket_stage0 to rocket_stage4 in this array via Inspector
    private int currentStage = 0;     // Track the current stage

    void Start()
    {
        // Ensure only the first stage is visible at the beginning
        for (int i = 1; i < rocketStages.Length; i++)
        {
            rocketStages[i].SetActive(false);
        }
        if (rocketStages.Length > 0)
            rocketStages[0].SetActive(true);
    }

    public void RocketUpgrade()
    {
        if (currentStage < rocketStages.Length - 1)
        {
            // Hide the current stage
            rocketStages[currentStage].SetActive(false);

            // Move to the next stage
            currentStage++;

            // Show the next stage
            rocketStages[currentStage].SetActive(true);
        }
        else
        {
            Debug.Log("Rocket is fully built!");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) // Press the "U" key
        {
            RocketUpgrade();
        }
    }
}
