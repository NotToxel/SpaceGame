// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    // Returns the ID for the Idle state
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    // Called when the AI enters the Idle state
    public void Enter(AiAgent agent)
    {
        // No specific behavior for entering the Idle state in this version
    }

    // Called when the AI exits the Idle state
    public void Exit(AiAgent agent)
    {
        // No specific behavior for exiting the Idle state in this version
    }

    // Called every frame while the AI is in the Idle state
    public void Update(AiAgent agent)
    {
        // Calculate the direction from the AI to the player
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;

        // If the player is further than the maximum sight distance, do nothing
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        // Get the forward direction of the agent
        Vector3 agentDirection = agent.transform.forward;

        // Normalize the player direction to make it a unit vector
        playerDirection.Normalize();

        // Calculate the dot product between the player direction and the agent's forward direction
        // A positive dot product means the player is in front of the agent
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);

        // If the player is in front of the agent, change the state to "ChasePlayer"
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }
}
