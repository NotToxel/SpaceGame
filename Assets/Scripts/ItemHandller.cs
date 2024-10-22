using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    //private GameObject player;
    public float pickupRange = 2.5f;
    public Transform holdPoint;
    private GameObject heldObject;
    public float throwForce = 10f;

    void Start() { 
        //player = GameObject.FindGameObjectWithTag("Player");
     }
 
    // Update is called once per frame
    void Update()
    {
        // Try to pick up an item if key is pressed
        if (Input.GetKeyDown(KeyCode.F)) {
            if (heldObject == null) { TryPickUPObject(); }
        }

        // Throw item when key is pressed
        if (Input.GetKeyDown(KeyCode.Q)) { DropObject(); }
    }

    // Attempt to pick up an object
    public void TryPickUPObject() {
        // Perform a raycast from the Player's camera to check if an object is within range
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange)) {
            if (hit.collider.CompareTag("Pickup")) {// Targeted object must have Pickup tag
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    // Pick up the object
    public void PickupObject (GameObject obj) {
        heldObject = obj;
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb != null) { objRb.isKinematic = true; }

        obj.transform.position = holdPoint.position;
        obj.transform.parent = holdPoint;
    }

    // Drop the object
    public void DropObject() {
        Rigidbody objRb = heldObject.GetComponent<Rigidbody>();
        if (objRb != null) { objRb.isKinematic = false; }
        heldObject.transform.parent = null;
        heldObject = null;
    }

    public GameObject getHeldObject() {
        return heldObject;
    }
}