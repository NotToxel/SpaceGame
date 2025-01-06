using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiRoamState : AiState
{
    private float timer = 0.0f;
    public float range = 20f; // Radius for random roaming
    public Transform centrePoint; // Centre point of the roaming area
    public AiStateId GetId()
    {
        return AiStateId.Roam;
    }
    public void Enter(AiAgent agent)
    {
        // Ensure agent is enabled
        if (agent == null || !agent.enabled)
        {
            return;
        }
        // Initialize the centrePoint (could be agent position or custom)
        centrePoint = agent.transform; // You can adjust this as necessary
        // Set the initial random destination
        SetRandomDestination(agent);
    }
    public void Exit(AiAgent agent)
    {
        // Optional: Any cleanup when exiting the roam state
    }
    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
        {
            SetRandomDestination(agent); // Pick a new random destination when the agent has reached the current one
        }
        if (timer < 0.0f)
        {
            float sqDistance = (agent.playerTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
            if (sqDistance > agent.config.maxDistance)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }
            timer = agent.config.maxTime;
        }
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }
        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }
    private void SetRandomDestination(AiAgent agent)
    {
        // Find a random point within the defined radius around the center
        Vector3 point;
        if (RandomPoint(agent.transform.position, range, out point))
        {
            agent.navMeshAgent.SetDestination(point);
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the point with gizmos
        }
    }
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Get a random point within the range
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        // Sample the NavMesh at the random point
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position; // Return the valid point on the NavMesh
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}