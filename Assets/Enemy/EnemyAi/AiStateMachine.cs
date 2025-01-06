// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiStateMachine
{
    public AiState[] states;           // Array to hold all the states available in the state machine
    public AiAgent agent;              // Reference to the AI agent (the entity controlled by the state machine)
    public AiStateId currentState;     // The current state that the AI is in

    // Constructor for the state machine, initializing the states array and storing the reference to the agent
    public AiStateMachine(AiAgent agent)
    {
        this.agent = agent;  // Store the reference to the agent
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length; // Get the number of states from the AiStateId enum
        states = new AiState[numStates];  // Initialize the states array with the number of states
    }

    // Registers a state in the state machine, associating it with its corresponding state ID
    public void RegisterState(AiState state)
    {
        int index = (int)state.GetId(); // Get the index of the state by casting the state ID to an integer
        states[index] = state;  // Assign the state to the corresponding position in the states array
    }

    // Retrieves the state corresponding to the provided AiStateId
    public AiState GetState(AiStateId stateId)
    {
        int index = (int)stateId;  // Get the index of the requested state from the state ID
        return states[index];  // Return the state at the corresponding index in the states array
    }

    // Updates the current state by calling its Update method (if it exists)
    public void Update()
    {
        GetState(currentState)?.Update(agent);  // Call the Update method on the current state, if it exists
    }

    // Changes the current state to a new state
    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.Exit(agent); // Call the Exit method on the current state before transitioning
        currentState = newState;  // Update the current state to the new state
        GetState(currentState).Enter(agent); // Call the Enter method on the new state to initialize it
    }
}
