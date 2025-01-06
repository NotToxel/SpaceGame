// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies; // Array to hold all the rigidbody components of the ragdoll
    Animator animator;       // Reference to the Animator component, to control the animation state

    // Start is called before the first frame update
    // Initializes the rigidbody array and animator. It also deactivates the ragdoll on startup.
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>(); // Get all Rigidbody components from children (ragdoll limbs)
        animator = GetComponent<Animator>();               // Get the Animator component attached to the object

        DeactiveRagdoll(); // Deactivate ragdoll at the start of the game
    }

    // DeactiveRagdoll disables the ragdoll physics by setting rigidbodies to be kinematic
    // and re-enables the regular animation (turning off ragdoll physics).
    public void DeactiveRagdoll()
    {
        // Loop through all rigidbodies in the ragdoll and set them to kinematic (no physics)
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true; // Set to kinematic so that physics are not applied to the ragdoll
        }

        animator.enabled = true; // Re-enable the animator so normal animation control takes over
    }

    // ActivateRagdoll enables ragdoll physics by disabling kinematic mode and disabling the Animator component
    public void ActivateRagdoll()
    {
        // Loop through all rigidbodies and disable kinematic mode to allow physics to control the ragdoll
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false; // Disable kinematic so physics are applied to the ragdoll
        }

        animator.enabled = false; // Disable the animator so it doesn't override the ragdoll physics
    }

    // ApplyForce applies a force to the ragdoll's hips (usually the root of the ragdoll) to simulate impact or knockback
    public void ApplyForce(Vector3 force)
    {
        // Get the Rigidbody attached to the "Hips" bone of the ragdoll (typically the root of the character)
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        // Apply the force to the Rigidbody with the given force mode
        rigidBody.AddForce(force, ForceMode.VelocityChange); // Apply the force instantly to simulate the knockback
    }
}
