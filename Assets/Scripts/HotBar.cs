using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    private ItemHandler itemHandler;
    public int hotbarSize = 9;
    public GameObject[] hotbarSlots;
    public int selectedSlot = 0;
    Color selectedColor;
    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {   
        gameObject.SetActive(false); // Hides the GameObject, delete later

        // initialize colours
        ColorUtility.TryParseHtmlString("#414141", out selectedColor);
        ColorUtility.TryParseHtmlString("#959595", out defaultColor);

        // Get ItemHandler methods
        itemHandler = GetComponent<ItemHandler>();

        UpdateHotbar(selectedSlot);
    }

    // Update is called once per frame
    void Update()
    {
        HandleHotbarInput();

        if (Input.GetKeyDown(KeyCode.F)) {
            if (itemHandler != null) {
                if (itemHandler.getHeldObject() == null){
                    if (hotbarSlots[selectedSlot] != null) {
                        itemHandler.PickupObject(hotbarSlots[selectedSlot]);
                    }
                }
                else {
                    itemHandler.DropObject();
                }
            }
        }
    }

    private void HandleHotbarInput() {
        // Selecting a slot with number keys (1-9)
        for (int i=0; i<hotbarSize; i++) {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i))) {
                selectedSlot = i;
                UpdateHotbar(selectedSlot);
            }
        }

        // Selecting a slot with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            if (scroll > 0f) { selectedSlot = nextSlot(selectedSlot); } 
            else if (scroll < 0f) { selectedSlot = prevSlot(selectedSlot); }

            UpdateHotbar(selectedSlot);
        }
    }

    int nextSlot(int slotNumber) {
        slotNumber++;
        return slotNumber%hotbarSize;
    }

    int prevSlot(int slotNumber) {
        slotNumber--;
        if (slotNumber<0) { slotNumber+=hotbarSize; }
        return slotNumber%hotbarSize;
    }

    void UpdateHotbar(int activeSlot) {
        for(int i=0; i<hotbarSize; i++) {
            Image slotImage = getSlotImage(i);
            if (slotImage != null) {
                slotImage.color = (i == activeSlot) ? selectedColor : defaultColor;
            }
        }
    }

    Image getSlotImage(int index) { return hotbarSlots[index].GetComponent<Image>(); }

}



