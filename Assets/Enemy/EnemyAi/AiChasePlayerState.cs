using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{


    float timer = 0.0f;
    private const float maxChaseDistance = 30f;
    private const float maxDistanceToPlayer = 30f;

    //float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        timer = agent.config.maxTime;
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
    }

    public void Exit(AiAgent agent)
    {

    }



    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

        if (distanceToPlayer > maxDistanceToPlayer)
        {
            agent.stateMachine.ChangeState(AiStateId.Roam);
            return;
        }

        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
        {
            agent.navMeshAgent.SetDestination(agent.playerTransform.position);
        }

        timer -= Time.deltaTime;

        if (timer < 0.0f)
        {
            timer = agent.config.maxTime;
        }
    }
}
