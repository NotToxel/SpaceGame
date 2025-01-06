using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 10f; // How high the player jumps
    [SerializeField] private Vector3 jumpDirection = Vector3.up; // Direction of the jump
    [SerializeField] private float jumpSpeed = 10f; // Speed at which the player moves

    private bool isPlayerOnPad = false; // Tracks if the player is on the jump pad
    private GameObject player; // Reference to the player GameObject

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            isPlayerOnPad = true;
            player = other.gameObject; // Store the reference to the player
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset when the player leaves the jump pad
        if (other.CompareTag("Player"))
        {
            isPlayerOnPad = false;
            player = null;
        }
    }

    private void Update()
    {
        // Check if the player is on the jump pad and presses the Space key
        if (isPlayerOnPad && Input.GetKeyDown(KeyCode.Space))
        {
            CharacterController controller = player.GetComponent<CharacterController>();

            if (controller != null)
            {
                // Calculate the jump velocity
                Vector3 jumpVelocity = jumpDirection.normalized * jumpSpeed;
                jumpVelocity.y = jumpHeight;

                // Apply the jump to the player's position
                StartCoroutine(ApplyJump(controller, jumpVelocity));
            }
        }
    }

    private System.Collections.IEnumerator ApplyJump(CharacterController controller, Vector3 jumpVelocity)
    {
        float elapsedTime = 0f;
        float jumpDuration = 1f; // Total duration of the jump effect (adjust as needed)

        while (elapsedTime < jumpDuration)
        {
            // Move the player using the calculated velocity
            controller.Move(jumpVelocity * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    // Optional: Visualize the jump direction in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, jumpDirection.normalized * 2f); // Visualize the jump direction
    }
}