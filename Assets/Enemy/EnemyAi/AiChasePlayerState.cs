// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    Animator animator;

    float timer = 0.0f;
    private const float maxChaseDistance = 30f;
    private const float maxDistanceToPlayer = 30f;
    private const float attackRange = 3f;
    private float cooldownTimer = 0.0f;
    private const float attackCooldown = 2.0f;

    // Method to get the ID for this state (ChasePlayer)
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    // Method that is called when the AI enters the ChasePlayer state
    public void Enter(AiAgent agent)
    {
        // Reset the timer based on the configuration from the agent
        timer = agent.config.maxTime;

        // Set the destination of the AI agent to the player's position
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);

        // Get the Animator component to control animations
        animator = agent.GetComponent<Animator>();
    }

    // Method that is called when the AI exits the ChasePlayer state
    public void Exit(AiAgent agent)
    {
        // No specific behavior for exiting this state in this version
    }

    // Method that is called every frame while the AI is in the ChasePlayer state
    public void Update(AiAgent agent)
    {
        // If the agent is not enabled, skip the update logic
        if (!agent.enabled)
        {
            return;
        }

        // Handle cooldown timer for attack actions
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Calculate the distance from the AI to the player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

        // If the player is too far, change the state to Roam
        if (distanceToPlayer > maxDistanceToPlayer)
        {
            agent.stateMachine.ChangeState(AiStateId.Roam);
            return;
        }

        // If the player is within attack range and the cooldown has elapsed, initiate an attack
        if (distanceToPlayer <= attackRange && cooldownTimer <= 0)
        {
            Attack(agent);
            return; // Exit the update function after triggering the attack
        }

        // If the AI has arrived at its destination, continue to chase the player
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
        {
            agent.navMeshAgent.SetDestination(agent.playerTransform.position);
        }

        // Decrease the timer, and reset it if it goes below zero
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            timer = agent.config.maxTime;
        }
    }

    // Method to handle the attack logic when the AI is close enough to the player
    private void Attack(AiAgent agent)
    {
        // Stop the AI from moving and set its destination to its current position
        agent.navMeshAgent.SetDestination(agent.transform.position);

        // Trigger the attack animation using the Animator
        animator.SetTrigger("Attack");

        // Reset the cooldown timer after the attack
        cooldownTimer = attackCooldown;

        // If the health bar object is assigned, apply damage to the player's health
        if (agent.healthBarObject != null)
        {
            HealthBar playerHealthBar = agent.healthBarObject.GetComponent<HealthBar>();

            // If a valid HealthBar component is found, apply damage
            if (playerHealthBar != null)
            {
                playerHealthBar.TakeDamage(10); // Damage value set to 10 for this example
                Debug.Log(playerHealthBar.health); // Log the player's health after taking damage
            }
            else
            {
                // Warn if the HealthBar component is not found
                Debug.LogWarning("HealthBar component not found on the HealthBar object.");
            }
        }
        else
        {
            // Warn if the HealthBar object is not assigned
            Debug.LogWarning("HealthBar object is not assigned.");
        }
    }
}
