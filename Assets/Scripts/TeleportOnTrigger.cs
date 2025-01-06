using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    // Define the position where the player will be teleported to
    [SerializeField] private Transform teleportDestination;

    // Tag of the player object
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private QuestCatalyst questCatalyst;
    
    [SerializeField] GameObject UiHealthBar;
    [SerializeField] private QuestCatalyst questCatalyst1;
    public GameObject dialogueTrigger;
    [SerializeField] private RocketBuilder rocketBuilder;
    private Inventory inventory;
        private void Start()
    {
        // Access the Inventory singleton
        inventory = Inventory.Instance;

        if (inventory == null)
        {
            Debug.LogError("Inventory instance not found!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the correct tag
        if (other.CompareTag(playerTag))
        {
            // Teleport the player to the teleportDestination
            other.transform.position = teleportDestination.position;
            other.transform.rotation = teleportDestination.rotation;
            UiHealthBar.SetActive(true);
            // Optionally, reset velocity if the player has a Rigidbody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
                        if (inventory != null && inventory.HasRequiredItems())
            {
                Debug.Log("Required items found! Upgrading rocket...");
                rocketBuilder.RocketUpgrade();
            }
            else
            {
                Debug.Log("Required items not found! Cannot upgrade the rocket.");
            }

            Debug.Log("Player teleported to: " + teleportDestination.position);
        }

        questCatalyst.CompleteQuest();
        dialogueTrigger.SetActive(true);
        questCatalyst1.CreateQuest();

    }
}