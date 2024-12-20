//Character Controller Inspired by Unity and samyam on Youtube
//https://youtu.be/5n_hmqHdijM?si=5ii-oBqL-S9Pn-7_

/*
This script manages the player's movement, interactions, and environmental response.
*/

using UnityEngine;
using System.Collections;


// Requires a CharacterController component to be attached to the same GameObject
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // --- Player movement settings --- 
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 5.0f; // Defaul walking speed
    [SerializeField] private float jumpHeight = 1.0f; // Height the player can jump
    [SerializeField] private float gravityValue = -9.81f; // Gravity applied to the player
    [SerializeField] private float normalHeight, crouchHeight; // Character heights for standing and crouching
    [SerializeField] private float crouchingSpeed = 2.0f; // Speed while crouching

    private float currentSpeed; // Current movement speed (varies based on crouching, walking or running)
    private bool groundedPlayer; // Tracks if the player is grounded
    private bool isCrouching = false; // Tracks if the player is currently crouching
    private Vector3 playerVelocity; // Tracks the player's vertical velocity


    // --- Interaction Settings ---
    [Header("Item Interaction")]
    [SerializeField] private float pickupRange = 2.5f; // Range within which objects can be picked up
    [SerializeField] private Transform holdPoint; // Position where held objects are placed
    [SerializeField] private float throwForce = 10f; // Force applied when throwing an object
    [SerializeField] private float puzzle1Range = 5f; // Interaction range for Puzzle1 objects

    private GameObject heldObject; // Currently held objects
    private int currentColorIndex = 0;
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };


    // --- Combat Settings ---
    [Header("Combat")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask attackLayer;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;


    // --- Components and References ---
    private CharacterController controller; // CharacterController component for movement
    private InputManager inputManager; // Input manager to handle player input
    private Transform cameraTransform; // Reference to the main camera's transform
    private HealthBar healthBar; // Reference to the player's health bar
    private Collider playerCollider; // Collider for the player (used to disable collision with held objects)
    private Coroutine crouchCoroutine; // Coroutine for smooth crouching transitions


    // --- Inventory Instances ---
    [Header("Hotbar")]
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private InventoryUI inventoryUI;
    private Inventory inventory;
    private bool inventoryIsOpen = false;

    private void Start()
    {
        //Initialize references
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance; // Get the input manager instance
        cameraTransform = Camera.main.transform; // Get the main camera's transform
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>(); // Get the player's collider

        currentSpeed = playerSpeed; // Set initial speed
        normalHeight = controller.height; // Store the default character height

        inventory = Inventory.Instance; // New instance of Inventory
        hotbar.SetInventory(inventory); // Setup the hotbar

        // --- Testing --- //
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 2), Quaternion.identity, new Item { itemType = Item.ItemType.Sword, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 1), Quaternion.identity, new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 3), Quaternion.identity, new Item { itemType = Item.ItemType.Sword, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 4), Quaternion.identity, new Item { itemType = Item.ItemType.Wrench, amount = 1 });
    }

    void Update()
    {
        HandleMovement();
        HandleInteraction();
        HandleCombat();
        ApplyGravity();
        HandleUI();
    }

    #region Movement
    private void HandleMovement()
    {
        // Ground check
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        // Movement input
        Vector2 movementInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Jumping
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        // Crouching
        if (inputManager.PlayerCrouchedThisFrame() != 0.0 && !isCrouching)
            ToggleCrouch(true);
        else if (inputManager.PlayerCrouchedThisFrame() == 0.0 && isCrouching)
            ToggleCrouch(false);
    }

    private void ToggleCrouch(bool crouch)
    {
        if (crouchCoroutine != null)
            StopCoroutine(crouchCoroutine);

        float targetHeight = crouch ? crouchHeight : normalHeight;
        float targetSpeed = crouch ? crouchingSpeed : playerSpeed;

        crouchCoroutine = StartCoroutine(CrouchTransition(targetHeight, targetSpeed));
        isCrouching = crouch;
    }

    private IEnumerator CrouchTransition(float targetHeight, float targetSpeed)
    {
        float initialHeight = controller.height;
        float initialSpeed = currentSpeed;
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            controller.height = Mathf.Lerp(initialHeight, targetHeight, elapsedTime / duration);
            currentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / duration);
            yield return null;
        }

        controller.height = targetHeight;
        currentSpeed = targetSpeed;
    }
    #endregion


    #region Interaction
    private void HandleInteraction()
    {
        if (inputManager.PlayerPickedItemUp())
            TryPickUpObject();

        if (inputManager.PlayerDroppedItem())
            DropObject();

        if (inputManager.PlayerInteract())
            InteractWithObject();
        //Debug.Log(inputManager.HotbarScrollSelect());
    }

    private void TryPickUpObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickup") || hit.collider.CompareTag("Sword"))
                PickupObject(hit.collider.gameObject);
        }
    }

    private void PickupObject(GameObject obj)
    {
        // Add the item to inventory
        hotbar.PickupItem(obj.GetComponent<Collider>());
        UpdateHeldItem();
    }

    private void DropObject()
    {
        // Remove the item from inventory
        hotbar.DropItem();
        UpdateHeldItem();
    }

    private void InteractWithObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, puzzle1Range))
        {
            if (hit.collider.CompareTag("Puzzle1"))
                CycleObjectColor(hit.collider.gameObject);
        }
    }

    private void CycleObjectColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            renderer.material.color = colors[currentColorIndex];
        }
    }

    private void UpdateHeldItem() {
        if (heldObject != null) {
            Destroy(heldObject);
        }

        GameObject prefab = hotbar.GetSelectedItemPrefab();
        if (prefab == null) { return; }

        heldObject = Instantiate(prefab);
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider objCollider = heldObject.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null)
            Physics.IgnoreCollision(playerCollider, objCollider, true);

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        if (heldObject.CompareTag("Sword"))
            heldObject.transform.localRotation = Quaternion.identity;
    }
    #endregion


    #region Combat
<<<<<<< HEAD
    private void HandleCombat() {
=======
    private void HandleCombat()
    {   
>>>>>>> parent of c0b2ed2 (Merge branch 'main' into bagrid3)
        //Debug.Log(hotbar.isHoldingWeapon());
        if (inputManager.PlayerLightAttack() && readyToAttack && hotbar.isHoldingWeapon() && !inventoryIsOpen)
            PerformLightAttack();
    }

    private void PerformLightAttack()
    {
        readyToAttack = false;
        Debug.Log("Performing light attack");
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        readyToAttack = true;
    }
    #endregion


    #region Utility
    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    #endregion


    #region UI
    private void HandleUI() {
        scrollSelectHotbar(inputManager.HotbarScrollSelect());
        numberSelectHotbar(inputManager.HotbarNumberSelect());
        if (inputManager.InventoryToggle())
            InventoryToggleInterface();
    }

    private void numberSelectHotbar(int keyPressed) {
        hotbar.HandleNumberKeyInput(keyPressed);
        UpdateHeldItem();
    }

    private void scrollSelectHotbar(float scrollValue) {
        hotbar.HandleScrollInput(scrollValue);
        UpdateHeldItem();
    }

    private void InventoryToggleInterface() {
        inventoryUI.ToggleInventory(inventoryIsOpen);
        inventoryIsOpen = !inventoryIsOpen;
    }
    #endregion
}