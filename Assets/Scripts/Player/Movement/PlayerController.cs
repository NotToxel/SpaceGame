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
    // Player movement settings
    [SerializeField] private float playerSpeed = 5.0f; // Defaul walking speed
    [SerializeField] private float currentSpeed; // Current movement speed (varies based on crouching, walking or running)
    [SerializeField] private float jumpHeight = 1.0f; // Height the player can jump
    [SerializeField] private float gravityValue = -9.81f; // Gravity applied to the player
    [SerializeField] private float normalHeight, crouchHeight; // Character heights for standing and crouching
    [SerializeField] private float playerCrouchingSpeed = 2.0f; // Speed while crouching

    // References to components and game objects
    private CharacterController controller; // CharacterController component for movement
    private Vector3 playerVelocity; // Tracks the player's vertical velocity
    private bool groundedPlayer; // Tracks if the player is grounded
    private bool crouching = false; // Tracks if the player is currently crouching
    private InputManager inputManager; // Input manager to handle player input
    private Transform cameraTransform; // Reference to the main camera's transform
    private HealthBar healthBar; // Reference to the player's health bar

    // Object interaction settings
    public float pickupRange = 2.5f; // Range within which objects can be picked up
    public Transform holdPoint; // Position where held objects are placed
    private GameObject heldObject; // Currently held objects
    public float throwForce = 10f; // Force applied when throwing an object
    private Collider playerCollider; // Collider for the player (used to disable collision with held objects)

    // Attacking behaviour
    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
    private bool attacking = false;
    private bool readyToAttack = true;
    int attackCount;

    // Coroutine for smooth crouching transitions
    private Coroutine crouchTransitionCoroutine;

    private void Start()
    {
        //Initialize variables and get references
        currentSpeed = playerSpeed; // Set initial speed
        controller = GetComponent<CharacterController>();
        normalHeight = controller.height; // Store the default character height
        inputManager = InputManager.Instance; // Get the input manager instance
        cameraTransform = Camera.main.transform; // Get the main camera's transform
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>(); // Get the player's collider
    }

    void Update()
    {
        // Check if the player is grounded and reset vertical velocity if true
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Handle player movement input
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x; // Align movement with camera direction
        controller.Move(move * Time.deltaTime * currentSpeed); // Apply movement with the current speed

        // Handle jumping
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue); // Calculate jump velocity
        }

        // Handle crouching
        if(inputManager.PlayerCrouchedThisFrame() != 0.0 && crouching == false)
        {
            StartCrouching(); // Begin crouching
        }

        // Handle standing up from crouch
        if(inputManager.PlayerCrouchedThisFrame() == 0.0 && crouching == true)
        {
            StopCrouching(); // End crouching
        }

        // Handle item pickup
        if (inputManager.PlayerPickedItemUp())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickupRange)) 
            {
                if (hit.collider.CompareTag("Pickup") || hit.collider.CompareTag("Sword") ) // Check if the object is tagges as "Pickup"
                {
                    PickupObject(hit.collider.gameObject); // Pick up the object
                }
            }
        }

        // Handle item drop
        if (inputManager.PlayerDroppedItem())
        {
            DropObject(); // Drop the held object
        }

        // Handle light attack
        if (inputManager.PlayerLightAttack())
        {
            LightAttack();
        }

        // Find the health bar component if it's not already asigned
        healthBar = FindFirstObjectByType<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar component not found on the player!");
        }
        
        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // Handle collisions with enemy
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SetActive(false);

            // Call TakeDamage from HealthBar when player collides with enemy
            if (healthBar != null)
            {
                healthBar.TakeDamage(70);  // Deal damage
                healthBar.EnterCombat();
            }
        }
    }

    // Pick up an object
    public void PickupObject (GameObject obj) {
        heldObject = obj;
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb != null) 
        { 
            objRb.isKinematic = true; // Make the object non-physical
        }

        // Disable collision between player and held object
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null) 
        {
            Physics.IgnoreCollision(playerCollider, objCollider, true);
        }

        obj.transform.position = holdPoint.position; // Place the objact at the hold point
        obj.transform.parent = holdPoint; // Make the object a child of the hold point
    }

    // Drop the held object
    public void DropObject() {
        Rigidbody objRb = heldObject.GetComponent<Rigidbody>();

        if (objRb != null) 
        { 
            objRb.isKinematic = false; // Re-enable physics
        }

        // Re-enable collision between player and held object
        Collider objCollider = heldObject.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null) 
        {
            Physics.IgnoreCollision(playerCollider, objCollider, false);
        }

        heldObject.transform.parent = null; // Detach the object from the player
        heldObject = null; // Clear the reference
    }

    // Start crouching
    private void StartCrouching()
    {
        if (crouchTransitionCoroutine != null)
        {
            StopCoroutine(crouchTransitionCoroutine); // Stop any ongoing crouch transition
        }

        crouchTransitionCoroutine = StartCoroutine(CrouchTransition(crouchHeight, playerCrouchingSpeed));
        crouching = true; // Set crouching state
    }

    // Stop crouching
    private void StopCrouching()
    {
        if (crouchTransitionCoroutine != null)
        {
            StopCoroutine(crouchTransitionCoroutine); // Stop any ongoing crouch transition
        }

        crouchTransitionCoroutine = StartCoroutine(CrouchTransition(normalHeight, playerSpeed));
        crouching = false; // Set standing state
    }

    // Smoothly transition between crouching and standing
    private IEnumerator CrouchTransition(float targetHeight, float targetSpeed)
    {
        float initialHeight = controller.height; // Initial character height
        float initialSpeed = currentSpeed; // Initial seed
        float elapsedTime = 0f;
        float duration = 0.3f; // Duration of the transition

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            controller.height = Mathf.Lerp(initialHeight, targetHeight, elapsedTime / duration); // Smoothly change height
            currentSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / duration); // Smoothly change speed
            yield return null;
        }

        // Ensure the final values are set after the transition
        controller.height = targetHeight;
        currentSpeed = targetSpeed;
    }

    private void LightAttack()
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true; 

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(PerformAttack), attackDelay);

    }

    private void PerformAttack()
    {
        // Detect enemies within range
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward); // Ray in the camera's forward direction
        if (Physics.Raycast(ray, out RaycastHit hit, attackDistance, attackLayer))
        {
            // Debug.Log($"Hit: {hit.collider.name}");

            // Deal damage if it's an enemy
            // EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            // if (enemyHealth != null)
            // {
            //     enemyHealth.TakeDamage(attackDamage); // Assume the enemy has a TakeDamage method
            //     // Debug.Log($"Dealt {attackDamage} damage to {hit.collider.name}");
            // }
        }

        // Optional: Add visual or sound effects here
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

}