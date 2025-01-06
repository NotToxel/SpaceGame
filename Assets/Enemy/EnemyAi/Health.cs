// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;                   // Maximum health of the AI agent
    [HideInInspector]
    public float currentHealth;               // Current health of the AI agent
    AiAgent agent;                            // Reference to the AI agent that owns this health component
    SkinnedMeshRenderer skinnedMeshRenderer;   // Renderer for the AI model, used to change color during damage (blink effect)
    UIHealthBar healthBar;                    // Reference to the UI health bar to visually update health
    public float blinkIntensity;              // Intensity of the color change when damage is taken
    public float blinkDuration;               // Duration for which the blink effect lasts
    float blinkTimer;                         // Timer to track how long the blink effect should last

    // Start is called before the first frame update
    // This method initializes the health variables, including setting the current health to max health and linking the health bar and renderer.
    void Start()
    {
        agent = GetComponent<AiAgent>();                           // Get the AiAgent attached to the object
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>(); // Get the SkinnedMeshRenderer for the blink effect
        healthBar = GetComponentInChildren<UIHealthBar>();         // Get the UIHealthBar component to display health
        currentHealth = maxHealth;                                 // Set the current health to the maximum value
    }

    // TakeDamage is called when the AI takes damage.
    // It reduces the current health, updates the health bar, and triggers the death sequence if health drops to zero or below.
    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;                                    // Decrease current health by the damage amount
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth); // Update the health bar UI with the new health percentage

        // Check if health is less than or equal to 0, which triggers the AI's death
        if (currentHealth <= 0.0f)
        {
            Die(direction);  // Call Die to handle the AI's death
        }

        blinkTimer = blinkDuration; // Reset the blink timer to start the blinking effect
    }

    // Die handles the AI's death logic.
    // It changes the state of the AI to "Death", hides the health bar, and plays any associated death effects.
    private void Die(Vector3 direction)
    {
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState; // Get the Death state from the state machine
        deathState.direction = direction; // Set the direction of death force
        agent.stateMachine.ChangeState(AiStateId.Death); // Change the AI's state to "Death"
        healthBar.gameObject.SetActive(false); // Hide the health bar UI upon death
    }

    // Update is called once per frame
    // This method handles the visual effect of blinking (flashing the AI's color) when damage is taken.
    private void Update()
    {
        blinkTimer -= Time.deltaTime; // Decrease the blink timer by the time elapsed
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration); // Calculate the lerp value based on remaining time for blinking
        float intensity = (lerp * blinkIntensity) + 1.0f; // Calculate the intensity of the blink effect
        skinnedMeshRenderer.material.color = Color.white * intensity; // Apply the color change to the mesh renderer (blinking effect)
    }
}
