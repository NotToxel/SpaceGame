// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public Vector3 direction;

    // Returns the ID for the Death state
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    // Called when the AI enters the Death state
    public void Enter(AiAgent agent)
    {
        // Activates the ragdoll effect on the AI (turns the AI into a physics-driven ragdoll)
        agent.ragdoll.ActivateRagdoll();

        // Apply an upward force to the ragdoll (to simulate a death reaction)
        direction.y = 1; // Set the vertical direction to 1 (upward)
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce); // Apply force with the specified intensity

        // Deactivate the UI health bar since the AI is dead
        agent.ui.gameObject.SetActive(false);

        // Ensure that the AI's mesh updates even when it's offscreen (useful for ragdoll or death state)
        agent.mesh.updateWhenOffscreen = true;
    }

    // Called when the AI exits the Death state
    public void Exit(AiAgent agent)
    {
        // No specific behavior for exiting this state in this version
    }

    // Called every frame while the AI is in the Death state
    public void Update(AiAgent agent)
    {
        // The Death state doesn't require updates, so it's left empty
    }
}
