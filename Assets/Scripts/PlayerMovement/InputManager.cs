//InputManager Inspired by samyam on Youtube
//https://youtu.be/5n_hmqHdijM?si=5ii-oBqL-S9Pn-7_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public bool CrouchIsPressed = false;
    public bool InventoryIsOpen = false;

    public static InputManager Instance{
        get{
            return _instance;
        }
    }

    private PlayerControls playerControls;

    private void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
        playerControls = new PlayerControls();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement(){
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta(){
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame(){
        return playerControls.Player.Jump.triggered;
    }

    public float PlayerCrouchedThisFrame(){
        return playerControls.Player.Crouch.ReadValue<float>();
    }
    
    public bool PlayerPickedItemUp(){
        return playerControls.Player.PickUpObject.triggered;
    }

    public bool PlayerDroppedItem(){
        return playerControls.Player.DropObject.triggered;
    }

    public bool PlayerLightAttack(){
        return playerControls.Player.LightAttack.triggered;
    }
    
    public bool PlayerHeavyAttack(){
        return playerControls.Player.HeavyAttack.triggered;
    }

    public bool PlayerInteract(){
        return playerControls.Player.Interact.triggered;
    }

    public float HotbarScrollSelect() {
        return playerControls.Player.ScrollSelectHotbarSlot.ReadValue<float>();
    }

    public int HotbarNumberSelect() {
        if (playerControls.Player.SelectHotbarSlot1.triggered) { return 1; }
        if (playerControls.Player.SelectHotbarSlot2.triggered) { return 2; }
        if (playerControls.Player.SelectHotbarSlot3.triggered) { return 3; }
        if (playerControls.Player.SelectHotbarSlot4.triggered) { return 4; }
        if (playerControls.Player.SelectHotbarSlot5.triggered) { return 5; }
        if (playerControls.Player.SelectHotbarSlot6.triggered) { return 6; }
        if (playerControls.Player.SelectHotbarSlot7.triggered) { return 7; }
        if (playerControls.Player.SelectHotbarSlot8.triggered) { return 8; }
        if (playerControls.Player.SelectHotbarSlot9.triggered) { return 9; }
        else { return -1; }
    }

    public bool InventoryToggle() {
        return playerControls.Player.Inventory.triggered;
<<<<<<< HEAD
    }
    
    public bool PlayerUsedBook(){
        return playerControls.Player.Book.triggered;
=======
>>>>>>> parent of c0b2ed2 (Merge branch 'main' into bagrid3)
    }
}
