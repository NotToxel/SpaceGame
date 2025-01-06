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

    //float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
        timer = agent.config.maxTime;
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
        animator = agent.GetComponent<Animator>();
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

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTransform.position);

        if (distanceToPlayer > maxDistanceToPlayer)
        {
            agent.stateMachine.ChangeState(AiStateId.Roam);
            return;
        }

        if (distanceToPlayer <= attackRange && cooldownTimer <= 0)
        {
            Attack(agent);
            return; // Exit the update function after triggering the attack
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

    private void Attack(AiAgent agent)
    {
        // Stop the NavMeshAgent from moving while attacking
        agent.navMeshAgent.SetDestination(agent.transform.position);

        // Trigger the attack animation in the animator
        animator.SetTrigger("Attack");

        // Start the cooldown timer to prevent immediate re-attacking
        cooldownTimer = attackCooldown;

        // Optionally, add additional logic for dealing damage or applying effects here
    }
}
