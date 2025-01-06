// Inspired by kiwicoder ai playlist https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Transform target; // The target object (e.g., AI agent) whose health bar is displayed
    public Image foregroundImage; // The image representing the foreground of the health bar (filled portion)
    public Image backgroundImage; // The image representing the background of the health bar (empty portion)
    public Vector3 offset; // Offset to position the health bar relative to the target
    public float displayDistance = 30f; // Maximum distance at which the health bar is visible

    // LateUpdate is called after all Update methods. It ensures the health bar is correctly positioned and visible each frame.
    private void LateUpdate()
    {
        // Calculate the distance between the target (AI agent) and the camera
        float distanceToTarget = Vector3.Distance(target.position, Camera.main.transform.position);

        // Check if the target is within the display distance range
        bool isWithinDisplayRange = distanceToTarget <= displayDistance;

        // If the target is within range, update the health bar's position and visibility
        if (isWithinDisplayRange)
        {
            // Calculate the direction from the camera to the target
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;

            // Check if the target is behind the camera (dot product of direction and camera forward is negative)
            bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;

            // Show or hide the health bar based on whether the target is behind the camera
            foregroundImage.enabled = !isBehind;
            backgroundImage.enabled = !isBehind;

            // Position the health bar above the target with the specified offset
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
        else
        {
            // Hide the health bar if the target is out of range
            foregroundImage.enabled = false;
            backgroundImage.enabled = false;
        }
    }

    // SetHealthBarPercentage adjusts the foreground image's width to reflect the target's health percentage
    public void SetHealthBarPercentage(float percentage)
    {
        // Get the width of the health bar container (parent object)
        float parentWidth = GetComponent<RectTransform>().rect.width;

        // Calculate the width of the foreground image based on the percentage
        float width = parentWidth * percentage;

        // Set the width of the foreground image to match the calculated value
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
