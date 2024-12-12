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
    private bool isRunning = false;
    private bool readyToJump = true;
    private Vector3 playerVelocity; // Tracks the player's vertical velocity
    private Quaternion currentRotation;


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
    public Transform cameraTransform; // Reference to the main camera's transform
    private HealthBar healthBar; // Reference to the player's health bar
    private Collider playerCollider; // Collider for the player (used to disable collision with held objects)
    private Coroutine crouchCoroutine; // Coroutine for smooth crouching transitions
    public GameObject holdingMelee; // Stores Melee player is currently holding

    private void Start()
    {
        //Initialize references
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance; // Get the input manager instance
        // cameraTransform = Camera.main.transform; // Get the main camera's transform
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>(); // Get the player's collider
        // animator = GetComponent<Animator>(); // Get the player's animator

        currentSpeed = playerSpeed; // Set initial speed
        normalHeight = controller.height; // Store the default character height
    }

    void Update()
    {
        HandleMovement();
        HandleInteraction();
        HandleCombat();

        if (attacking && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= animationFinishTime)
        {
            attacking = false;
        }

        ApplyGravity();
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

        // Turn(move);
        AnimateRun(move);

        // Jumping
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
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

    private IEnumerator InitialiseJump(){
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
            DropMelee(); // Implement tryDropItem which identifies what kind of item player is holding.

        if (inputManager.PlayerInteract())
            InteractWithObject();
    }

    private void TryPickUpObject()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, pickupRange))
        {
            // if (hit.collider.CompareTag("Pickup")){
            //     PickupObject(hit.collider.gameObject); 
            // }

            if (hit.collider.CompareTag("Knife")) 
            {
                PickUpMelee(hit.collider.gameObject);
                holdingWeapon = true;
                animator.SetBool("isHoldingMelee", true);
            }
        }
    }

    private void PickUpMelee(GameObject melee)
    {
        Rigidbody rb = melee.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider meleeCollider = melee.GetComponent<Collider>();
        if (meleeCollider != null && playerCollider != null)
            Physics.IgnoreCollision(playerCollider, meleeCollider, true);

        melee.transform.SetParent(holdPoint);
        melee.transform.localPosition = Vector3.zero;
        
        melee.transform.localRotation = Quaternion.identity;

        heldObject = melee;
        melee.SetActive(false);
        holdingMelee.SetActive(true);
    }

    private void DropMelee()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        Collider objCollider = heldObject.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null)
            Physics.IgnoreCollision(playerCollider, objCollider, false);

        heldObject.SetActive(true);
        holdingMelee.SetActive(false);

        heldObject.transform.parent = null;
        heldObject = null;

        animator.SetBool("isHoldingMelee", false);
    }

    // private void PickupObject(GameObject obj)
    // {
    //     Rigidbody rb = obj.GetComponent<Rigidbody>();
    //     if (rb != null) rb.isKinematic = true;

    //     Collider objCollider = obj.GetComponent<Collider>();
    //     if (objCollider != null && playerCollider != null)
    //         Physics.IgnoreCollision(playerCollider, objCollider, true);

    //     obj.transform.SetParent(holdPoint);
    //     obj.transform.localPosition = Vector3.zero;
    //     if (obj.CompareTag("Knife"))
    //         obj.transform.localRotation = Quaternion.identity;

    //     heldObject = obj;
    //     obj.SetActive(false);
    // }

    // private void DropObject()
    // {
    //     if (heldObject == null) return;

    //     Rigidbody rb = heldObject.GetComponent<Rigidbody>();
    //     if (rb != null) rb.isKinematic = false;

    //     Collider objCollider = heldObject.GetComponent<Collider>();
    //     if (objCollider != null && playerCollider != null)
    //         Physics.IgnoreCollision(playerCollider, objCollider, false);

    //     heldObject.SetActive(true);
    //     holdingKnife.SetActive(false);
    //     heldObject.transform.parent = null;
    //     heldObject = null;
    //     holdingWeapon = false;
    //     animator.SetBool("isHoldingWeapon", false);
    // }

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
    #endregion


    #region Combat
    private void HandleCombat()
    {
        if (inputManager.PlayerLightAttack() && readyToAttack && holdingWeapon)
            PerformLightAttack();
    }

    private void PerformLightAttack()
    {
        readyToAttack = false;
        attacking = true;
        animator.SetTrigger("lightAttack");
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
}