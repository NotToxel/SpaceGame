using UnityEngine;

public class RotateTextWithPlayerView : MonoBehaviour
{
    private Camera playerCamera;

    void Start()
    {
        // Get the main camera (assumed to be the player's camera)
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            // Rotate the text to face the player's camera
            Vector3 directionToCamera = playerCamera.transform.position - transform.position;

            // Make the text face towards the camera
            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);

            // Apply the rotation to the text
            transform.rotation = targetRotation;

            // Optional: Adjust to fix mirroring issues
            transform.Rotate(0, 180, 0);
        }
    }
}
