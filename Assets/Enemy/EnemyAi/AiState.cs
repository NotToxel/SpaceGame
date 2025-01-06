// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum to represent different states for the AI
public enum AiStateId
{
    ChasePlayer, // AI is chasing the player
    Death,       // AI is in the death state
    Idle,        // AI is idle and not moving
    Roam         // AI is roaming in a random pattern
}

// Interface that defines the contract for AI states
public interface AiState
{
    // Returns the unique identifier for the state
    AiStateId GetId();

    // Called when the AI enters the state, initializing any necessary parameters or behaviors
    void Enter(AiAgent agent);

    // Called every frame while the AI is in the state to update the behavior
    void Update(AiAgent agent);

    // Called when the AI exits the state to perform any necessary cleanup
    void Exit(AiAgent agent);
}
