//Character Controller Inspired by Unity and samyam on Youtube
//https://youtu.be/5n_hmqHdijM?si=5ii-oBqL-S9Pn-7_

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float normalHeight, crouchHeight;
    [SerializeField] private float playerCrouchingSpeed = 1.0f;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool crouching = false;
    private InputManager inputManager;
    private Transform cameraTransform;
    private HealthBar healthBar;

    public float pickupRange = 2.5f;
    public Transform holdPoint;
    private GameObject heldObject;
    public float throwForce = 10f;
    private Collider playerCollider;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        normalHeight = controller.height;
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if(inputManager.PlayerCrouchedThisFrame() != 0.0 && crouching == false){
            controller.height = crouchHeight;
            controller.Move(move * Time.deltaTime * playerCrouchingSpeed);
            Debug.Log("Crouched"); //TESTING
            crouching = true;
        }

        if(inputManager.PlayerCrouchedThisFrame() == 0.0 && crouching == true){
            controller.height = normalHeight;
            Debug.Log("Uncrouched"); //TESTING
            crouching = false;
        }

        if (inputManager.PlayerPickedItemUp())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickupRange)) {
                if (hit.collider.CompareTag("Pickup")) {// Targeted object must have Pickup tag
                    PickupObject(hit.collider.gameObject);
                }
            }
        }

        if (inputManager.PlayerDroppedItem())
        {
            DropObject();
        }

        healthBar = FindFirstObjectByType<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar component not found on the player!");
        }
        
    
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

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

    public void PickupObject (GameObject obj) {
        heldObject = obj;
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb != null) { objRb.isKinematic = true; }

        // Disable collision between player and held object
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null) {
            Physics.IgnoreCollision(playerCollider, objCollider, true);
        }

        obj.transform.position = holdPoint.position;
        obj.transform.parent = holdPoint;
    }

    public void DropObject() {
        Rigidbody objRb = heldObject.GetComponent<Rigidbody>();

        if (objRb != null) { 
            objRb.isKinematic = false; 
        }

        // Re-enable collision between player and held object
        Collider objCollider = heldObject.GetComponent<Collider>();
        if (objCollider != null && playerCollider != null) {
            Physics.IgnoreCollision(playerCollider, objCollider, false);
        }

        heldObject.transform.parent = null;
        heldObject = null;
    }
}