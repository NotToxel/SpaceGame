// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    // Public variables for different components and settings
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;
    public Transform playerTransform;
    public HealthBar healthBar;
    public GameObject healthBarObject;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the ragdoll, mesh, UI, and other components
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthBar = GetComponent<HealthBar>();

        // If playerTransform is not assigned, try to find the player by tag
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        // Initialize the state machine and register the possible states
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiRoamState());

        // Set the initial state for the AI agent
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the state machine, allowing it to transition between states and execute behavior
        stateMachine.Update();
    }
}