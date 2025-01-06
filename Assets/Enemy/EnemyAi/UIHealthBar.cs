using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Transform target; // Target to track (e.g., AI agent)
    public Image foregroundImage;
    public Image backgroundImage;
    public Vector3 offset;
    public float displayDistance = 30f; // Maximum distance to display the health bar

    private void LateUpdate()
    {
        // Calculate the distance between the target (AI agent) and the player
        float distanceToTarget = Vector3.Distance(target.position, Camera.main.transform.position);

        // Check if the target (AI) is within the display distance
        bool isWithinDisplayRange = distanceToTarget <= displayDistance;

        // If the target is within range, update the position and visibility of the health bar
        if (isWithinDisplayRange)
        {
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;
            bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;

            // Show or hide the health bar based on whether the target is behind the camera
            foregroundImage.enabled = !isBehind;
            backgroundImage.enabled = !isBehind;

            // Position the health bar above the target with the given offset
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
        else
        {
            // Hide the health bar if the target is out of range
            foregroundImage.enabled = false;
            backgroundImage.enabled = false;
        }
    }

    public void SetHealthBarPercentage(float percentage)
    {
        // Adjust the width of the health bar foreground based on the percentage
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}