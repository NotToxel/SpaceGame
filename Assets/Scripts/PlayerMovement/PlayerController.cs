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
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool crouching = false;
    private InputManager inputManager;
    private Transform cameraTransform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        normalHeight = controller.height;
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
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
            Debug.Log("Crouched"); //TESTING
            crouching = true;
        }

        if(inputManager.PlayerCrouchedThisFrame() == 0.0 && crouching == true){
            controller.height = normalHeight;
            Debug.Log("Uncrouched"); //TESTING
            crouching = false;
        }
    
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}