using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public GameObject floatingTextPrefab; // Assign your floating text prefab
    public float maxDistance = 5f; // Maximum distance for the text to appear
    private GameObject floatingTextInstance;
    private Camera playerCamera; // Reference to the player's camera
    // Start is called before the first frame update

    public float rotationSpeed = 5f;
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerLookingAtObject();
    }

    private void CheckPlayerLookingAtObject()
    {
        // Raycast from the center of the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the ray hit this object
            if (hit.transform == transform)
            {
                ShowFloatingText();
                return; // Exit to avoid destroying the text below
            }
        }

        // If not looking at the object, hide the text
        HideFloatingText();
    }

    private void ShowFloatingText()
    {
        if (floatingTextInstance == null) // Ensure it only spawns once
        {
            // Offset the position to make the text appear above the object
            Vector3 abovePosition = transform.position + Vector3.up; // Adjust "2f" as needed for height
            floatingTextInstance = Instantiate(floatingTextPrefab, abovePosition, Quaternion.identity, transform);
        }
    }


    private void HideFloatingText()
    {
        if (floatingTextInstance != null)
        {
            Destroy(floatingTextInstance);
            floatingTextInstance = null;
        }
    }
}