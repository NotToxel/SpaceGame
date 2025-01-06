// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    // The maximum time value used in the AI logic, could be used for timing events or behavior transitions
    public float maxTime = 1.0f;

    // The maximum distance used in AI logic, might be for checking distances between the AI and other objects or the player
    public float maxDistance = 1.0f;

    // The force applied when the AI dies, likely used for ragdoll or physical effects upon death
    public float dieForce = 10;

    // The maximum sight distance the AI can 'see' or detect the player or other objects
    public float maxSightDistance = 5.0f;
}
