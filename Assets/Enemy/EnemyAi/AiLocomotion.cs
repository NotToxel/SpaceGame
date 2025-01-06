// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    // Reference to the NavMeshAgent component to control movement
    NavMeshAgent agent;

    // Reference to the Animator component to control animations
    Animator animator;

    // Start is called before the first frame update
    // Initializes the NavMeshAgent and Animator components
    void Start()
    {
        // Get the NavMeshAgent component attached to the GameObject
        agent = GetComponent<NavMeshAgent>();

        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // Updates the speed parameter in the Animator based on the agent's movement
    void Update()
    {
        // Check if the agent has a path (i.e., if it's moving towards a destination)
        if (agent.hasPath)
        {
            // Set the "Speed" parameter in the Animator to the magnitude of the agent's velocity
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            // If the agent doesn't have a path, set the "Speed" parameter to 0 (idle)
            animator.SetFloat("Speed", 0);
        }
    }
}
