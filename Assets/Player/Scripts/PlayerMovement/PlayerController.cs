//Character Controller Inspired by Unity and samyam on Youtube
//https://youtu.be/5n_hmqHdijM?si=5ii-oBqL-S9Pn-7_

/*
This script manages the player's movement, interactions, and environmental response.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Requires a CharacterController component to be attached to the same GameObject
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // --- Player movement settings --- 
    [Header("Player Movement")]
    public float playerSpeed = 5.0f; // Default walking speed
    [SerializeField] private float jumpHeight = 1.0f; // Height the player can jump
    [SerializeField] private float sprintingJumpHeight = 2.0f; // Height the player can jump while sprinting
    [SerializeField] private float gravityValue = -9.81f; // Gravity applied to the player
    [SerializeField] private float normalHeight, crouchHeight; // Character heights for standing and crouching
    [SerializeField] private float crouchingSpeed = 2.0f; // Speed while crouching

    public float sprintingSpeed = 10f;
    public float currentSpeed; // Current movement speed (varies based on crouching, walking or running)
    public bool groundedPlayer; // Tracks if the player is grounded
    private bool isCrouching = false; // Tracks if the player is currently crouching
    public bool isRunning = false;
    public bool isSprinting = false;
    private bool readyToJump = true;
    private Vector3 playerVelocity; // Tracks the player's vertical velocity
    private Quaternion currentRotation;


    // --- Interaction Settings ---
    [Header("Item Interaction")]
    [SerializeField] private float pickupRange = 2.5f; // Range within which objects can be picked up
    [SerializeField] private Transform holdPoint; // Position where held objects are placed
    [SerializeField] private float throwForce = 10f; // Force applied when throwing an object
    // [SerializeField] private float puzzle1Range = 5f; // Interaction range for Puzzle1 objects
    private bool isTabletOpen = false;
    public float interactingRange = 5f;
    public bool isInteracting;

    private GameObject heldObject; // Currently held objects
    // private int currentColorIndex = 0;
    // private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };


    // --- Combat Settings ---
    [Header("Combat")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackSpeed = 0.1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask attackLayer;

    private bool attacking = false;
    private bool readyToAttack = true;
    private bool holdingWeapon = false;
    private int attackCount;

    // --- Animation Settings ---
    [Header("Animation")]
    public Animator animator;
    [SerializeField] private float animationFinishTime = 0.9f;

    // --- Components and References ---
    private CharacterController controller; // CharacterController component for movement
    private InputManager inputManager; // Input manager to handle player input
    public GameObject camera; // Reference to main camera GameObject
    public Transform cameraTransform; // Reference to the main camera's transform
    private HealthBar healthBar; // Reference to the player's health bar
    private Collider playerCollider; // Collider for the player (used to disable collision with held objects)
    private Coroutine crouchCoroutine; // Coroutine for smooth crouching transitions
    public GameObject holdingMelee; // Stores Melee player is currently holding
    public GameObject tablet;
    public QuestTabletButton questTabletButton;
    public DialogueManager dialogueManager;
    // public Book bookScript;


    // --- Inventory Instances ---
    [Header("Hotbar")]
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private InventoryUI inventoryUI;
    private Inventory inventory;
    private bool inventoryIsOpen = false;

    // --- Audio ---
    [Header("Audio")]
    public AudioManager audioManager;

    // --- NPCs ---
    [Header("NPCs")]
    [SerializeField] private Chip chip;

    //TESTING
    public GameObject dialogueTrigger;

    private void Start()
    {
        //Initialize references
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance; // Get the input manager instance
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>(); // Get the player's collider

        currentSpeed = playerSpeed; // Set initial speed
        normalHeight = controller.height; // Store the default character height

        inventory = Inventory.Instance; // New instance of Inventory
        hotbar.SetInventory(inventory); // Setup the hotbar

        // --- Testing --- //
    }

    void Update()
    {
        HandleInteraction();
        if (dialogueManager.inDialogue == false) 
        {
            HandleMovement();
            HandleCombat();

            if (attacking && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= animationFinishTime)
            {
                attacking = false;
            }

            ApplyGravity();
            HandleUI();
        }

        if (dialogueManager.inDialogue == true)
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
            isCrouching = false;
            isSprinting = false;
        }
    }

    #region Movement
    private void HandleMovement()
    {
        // Ground check
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;


        if (isRunning == false && holdingWeapon) { animator.SetBool("isHoldingMelee", true); }
        if (isRunning == true){animator.SetBool("isHoldingMelee", false); }
        

        // Movement input
        if (inputManager.PlayerIsSprinting() != 0.0f && !isSprinting)
        {
            currentSpeed = sprintingSpeed;
            isSprinting = true;
        } 
        else if (inputManager.PlayerIsSprinting() == 0.0 && isSprinting)
        {
            currentSpeed = playerSpeed;
            isSprinting = false;
        }

        Vector2 movementInput = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        AnimateRun(move);
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Jumping
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            if (!isSprinting)
            {
                playerVelocity.y += Mathf.Sqrt(sprintingJumpHeight * -3.0f * gravityValue);
            }
            else
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            groundedPlayer = false;
            animator.SetTrigger("isJumping");
            StartCoroutine(InitialiseJump());
        }

        // Crouching
        if (inputManager.PlayerCrouchedThisFrame() != 0.0 && !isCrouching)
            ToggleCrouch(true);
        else if (inputManager.PlayerCrouchedThisFrame() == 0.0 && isCrouching)
            ToggleCrouch(false);
    }

    // private void ToggleSprint(bool sprint)
    // {
    //     // if (crouchCoroutine != null)
    //     //     StopCoroutine(crouchCoroutine);

    //     float targetSpeed = spint ? sprintingSpeed : playerSpeed;

    //     crouchCoroutine = StartCoroutine(CrouchTransition(targetHeight, targetSpeed));
    //     isCrouching = crouch;
    // }

    private IEnumerator InitialiseJump()
    {
        yield return new WaitForSeconds(0.1f);
        groundedPlayer = true;
    }

    void AnimateRun(Vector3 direction)
    {
        isRunning = (direction.x > 0.1f || direction.x < -0.1f) || (direction.z > 0.1f || direction.z < -0.1f) ? true : false;
        animator.SetBool("isRunning", isRunning);
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
            DropObject(); // Implement tryDropItem which identifies what kind of item player is holding.

        if (inputManager.PlayerInteract() == true)
            TriggerDialogue(dialogueTrigger);

        if (inputManager.PlayerUsedTablet())
            InteractWithTablet();

        if (inputManager.PlayerContinuesDialogue() && dialogueManager.inDialogue == true)
            NextSentence();

        if (inputManager.PlayerInteract() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, pickupRange)) {
            if (hit.collider.gameObject.CompareTag("Chip")) {
                    Debug.Log("Interacting with Chip");
                    chip.Interact();
            }
        }
    }

    private void TriggerDialogue(GameObject dialogueTrigger)
    {
        Debug.Log("triggering");
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, interactingRange))
        {
            Debug.Log("hit");
            if (hit.collider.CompareTag("NPC"))
                InteractWithNPC(hit.collider.gameObject, dialogueTrigger);
        }
    }

    private void InteractWithNPC(GameObject obj, GameObject dialogueTrigger)
    {
        dialogueTrigger.SetActive(true);   
    }

       // private void InteractWithObject()
    // {
    //     if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, puzzle1Range))
    //     {
    //         if (hit.collider.CompareTag("Puzzle1"))
    //             CycleObjectColor(hit.collider.gameObject);
    //     }
    // }

    // private void CycleObjectColor(GameObject obj)
    // {
    //     Renderer renderer = obj.GetComponent<Renderer>();
    //     if (renderer != null)
    //     {
    //         currentColorIndex = (currentColorIndex + 1) % colors.Length;
    //         renderer.material.color = colors[currentColorIndex];
    //     }
    // }

    private void NextSentence()
    {
        //Debug.Log("Displaying next");
        dialogueManager.DisplayNextSentence();
    }

    private void InteractWithTablet()
    {
        FirstPersonCamera cameraScript = FindObjectOfType<FirstPersonCamera>();
        if (isTabletOpen)
        {
            if (cameraScript != null)
            {
                cameraScript.EnableCam();
            }

            audioManager.PlaySFX(audioManager.closeTablet);
            tablet.SetActive(false);
            isTabletOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {   
            if (cameraScript != null)
            {
                cameraScript.DisableCam();
            }

            audioManager.PlaySFX(audioManager.openTablet);
            questTabletButton.OpenQuestTablet();
            tablet.SetActive(true);
            isTabletOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void TryPickUpObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Item")){
                PickupObject(hit.collider.gameObject); 
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void PickupObject(GameObject obj)
    {
        hotbar.PickupItem(obj.GetComponent<Collider>());
        UpdateHeldItem();
    }

    private void DropObject()
    {
        hotbar.DropItem();
        UpdateHeldItem();
    }

    // private void InteractWithObject()
    // {
    //     if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, puzzle1Range))
    //     {
    //         if (hit.collider.CompareTag("Puzzle1"))
    //             CycleObjectColor(hit.collider.gameObject);
    //     }
    // }

    // private void CycleObjectColor(GameObject obj)
    // {
    //     Renderer renderer = obj.GetComponent<Renderer>();
    //     if (renderer != null)
    //     {
    //         currentColorIndex = (currentColorIndex + 1) % colors.Length;
    //         renderer.material.color = colors[currentColorIndex];
    //     }
    // }

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
        heldObject.transform.localRotation = Quaternion.identity;
        //if (heldObject.CompareTag("Sword"))
        //    heldObject.transform.localRotation = Quaternion.identity;
    }
    #endregion


    #region Combat
    private void HandleCombat(){
        //Debug.Log(hotbar.isHoldingWeapon());
        if (inputManager.PlayerLightAttack() && readyToAttack && (hotbar.isHoldingWeapon() || hotbar.isBareFist()) && !inventoryIsOpen)
            PerformLightAttack();
    }

    private void PerformLightAttack()
    {
        readyToAttack = false;
        attacking = true;
        animator.SetTrigger("lightAttack");
        StartCoroutine(AttackCooldown());
        //Debug.Log("Performing Light Attack");
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
        //Debug.Log("Toggling Inventory");
        if (inventoryIsOpen) { inventoryUI.closeInventory(); }
        else { inventoryUI.openInventory(); }
        inventoryIsOpen = inventoryUI.IsOpen();
    }
    #endregion
}