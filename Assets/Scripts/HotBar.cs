using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    public int hotbarSize = 9;
    public Image[] hotbarSlots;
    public int selectedSlot = 0;

    // Start is called before the first frame update
    void Start()
    {   
        UpdateHotbar();
    }

    // Update is called once per frame
    void Update()
    {
        HandleHotbarInput();
    }

    private void HandleHotbarInput() {
        // Selecting a slot with number keys (1-9)
        for (int i=0; i<hotbarSize; i++) {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i))) {
                selectedSlot = i;
                UpdateHotbar();
            }
        }

        // Selecting a slot with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            if (scroll > 0f) {
                selectedSlot--;
            } else if (scroll < 0f) {
                selectedSlot++;
            }

            // Wrap around if we are on the first/last slot
            if (selectedSlot < 0) {
                selectedSlot = hotbarSize - 1;
            } else if (selectedSlot > 0) {
                selectedSlot = 0;
            }
            
            UpdateHotbar();
        }
    }
    

    void UpdateHotbar() {
        for(int i=0; i<hotbarSize; i++) {
            Transform highlight = hotbarSlots[i].transform.Find("Highlight");
            if(highlight != null) {
                highlight.gameObject.SetActive(i == selectedSlot);
            }
        }
    }

}



