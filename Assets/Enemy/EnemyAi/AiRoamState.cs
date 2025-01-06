// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiRoamState : AiState
{
    private float timer = 0.0f;
    public float range = 20f; // Radius for random roaming
    public Transform centrePoint; // Centre point of the roaming area

    // Returns the ID for the Roam state
    public AiStateId GetId()
    {
        return AiStateId.Roam;
    }

    // Called when the AI enters the Roam state
    public void Enter(AiAgent agent)
    {
        // Ensure the agent is enabled before proceeding
        if (agent == null || !agent.enabled)
        {
            return;
        }

        // Initialize the centrePoint (you can set it to the agent's position or a custom point)
        centrePoint = agent.transform; // You can adjust this as needed

        // Set the initial random destination for the agent
        SetRandomDestination(agent);
    }

    // Called when the AI exits the Roam state
    public void Exit(AiAgent agent)
    {
        // Optional cleanup when exiting the Roam state
        // You can add any necessary logic to reset or stop roaming here if needed
    }

    // Called every frame while the AI is in the Roam state
    public void Update(AiAgent agent)
    {
        // Ensure the agent is still enabled before proceeding
        if (!agent.enabled)
        {
            return;
        }

        // Decrease the timer by the time passed since the last frame
        timer -= Time.deltaTime;

        // Check if the agent has reached its destination (remaining distance is within stopping distance)
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
        {
            // Set a new random destination when the agent reaches the current one
            SetRandomDestination(agent);
        }

        // If the timer is less than 0.0f, check the distance to the player and set a new destination if necessary
        if (timer < 0.0f)
        {
            // Calculate the squared distance to the player (avoids using sqrt for optimization)
            float sqDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;

            // If the player is beyond the max distance, move towards the player
            if (sqDistance > agent.config.maxDistance)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }

            // Reset the timer to the configured max time
            timer = agent.config.maxTime;
        }

        // Calculate the direction from the agent to the player
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;

        // If the player is out of the AI's sight range, do nothing
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        // Get the forward direction the agent is facing
        Vector3 agentDirection = agent.transform.forward;

        // Normalize the player direction vector (to make it a unit vector)
        playerDirection.Normalize();

        // Calculate the dot product between the player's direction and the agent's forward direction
        // If the player is in front of the agent, the dot product will be positive
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);

        // If the player is in front of the agent, change the state to "ChasePlayer"
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    // Sets a random destination for the agent within a defined range around the center point
    private void SetRandomDestination(AiAgent agent)
    {
        // Find a random point within the defined radius around the center
        Vector3 point;
        if (RandomPoint(agent.transform.position, range, out point))
        {
            // Set the random point as the agent's new destination
            agent.navMeshAgent.SetDestination(point);

            // Visualize the random point with a blue ray for debugging
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        }
    }

    // Finds a random valid point on the NavMesh within the specified range
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Get a random point within the range
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        // Sample the NavMesh at the random point to find a valid position
        NavMeshHit hit;

        // If a valid point on the NavMesh is found, return true with the position
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position; // Return the valid point on the NavMesh
            return true;
        }

        // If no valid point was found, return false with a zero vector
        result = Vector3.zero;
        return false;
    }
}
