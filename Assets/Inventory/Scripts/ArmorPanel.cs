// Script reference: https://www.youtube.com/watch?v=2WnAOV7nHW0

using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmorPanel : MonoBehaviour
{
    [SerializeField] private Transform armorSlotContainer;
    [SerializeField] private Transform armorSlotTemplate;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    private Inventory inventory = Inventory.Instance;
    private UIManager uiManager;

    private int armorSlots = 4;
    private const int helmetIndex = 45;
    private const int chestIndex = 46;
    private const int legsIndex = 47;
    private const int bootsIndex = 48;
    private Item currentHelmet;
    private Item currentChest;
    private Item currentLegs;
    private Item currentBoots;



    void Awake() {
        // Initialise uiManager reference
        uiManager = FindObjectOfType<UIManager>();
    }
    
    void Start() {
        RefreshArmorPanel();
        //helmetIndex = inventory.GetMaxSize();
        //chestIndex = inventory.GetMaxSize()+1;
        //legsIndex = inventory.GetMaxSize()+2;
        //bootsIndex = inventory.GetMaxSize()+3;
        //Debug.Log(helmetIndex);
        //Debug.Log(chestIndex);
        //Debug.Log(legsIndex);
        //Debug.Log(bootsIndex);
    }

    public void RefreshArmorPanel() {
        // Generate slots
        int x = 0;
        int y = 0;
        float armorSlotCellSize = 100f;
        for (int i=0; i<armorSlots; i++) {
            int armorSlotIndex = inventory.GetMaxSize()+i;

            // Instantiate a new item slot for each item beyond the hotbar
            RectTransform armorSlotRectTransform = Instantiate(armorSlotTemplate, armorSlotContainer).GetComponent<RectTransform>();
            armorSlotRectTransform.gameObject.SetActive(true);
            armorSlotRectTransform.anchoredPosition = new Vector2(x * armorSlotCellSize, y * armorSlotCellSize);
            y--;

            // Find and set the image to the item's sprite
            Image image = armorSlotRectTransform.Find("image").GetComponent<Image>();
            switch(armorSlotIndex) {
                case helmetIndex:
                    if (currentHelmet == null) { break; }
                    image.sprite = currentHelmet.GetSprite(); break;
                case chestIndex:
                    if (currentChest == null) { break; }
                    image.sprite = currentChest.GetSprite(); break;
                case legsIndex:
                    if (currentLegs == null) { break; }
                    image.sprite = currentLegs.GetSprite(); break;
                case bootsIndex:
                    if (currentBoots == null) { break; }
                    image.sprite = currentBoots.GetSprite(); break;
                    
            }
    

            // Set border colors
            Image border = armorSlotRectTransform.Find("border").GetComponent<Image>();
            border.color = uiManager.SetBorderColor(border, armorSlotIndex);

            // Add button listeners
            Button button = armorSlotRectTransform.Find("border").GetComponent<Button>();
            if (button != null)
            {
                // Listen for left click
                button.onClick.AddListener(() => uiManager.SlotClicked(armorSlotIndex));

                // Listen for right click
                EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry rightClickEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };

                // Check if the right mouse button was clicked
                rightClickEntry.callback.AddListener((data) =>
                    {
                    PointerEventData pointerData = (PointerEventData)data;
                    if (pointerData.button == PointerEventData.InputButton.Right)
                    {
                        Unequip(armorSlotIndex);
                    }
                    });

                eventTrigger.triggers.Add(rightClickEntry);
            }
            else
            {
                Debug.LogError("Button component not found in itemSlotTemplate.");
            }

        }
        //Debug.Log("Initialising armor panel");
    }
    public Item GetEquippedArmor(int index) {
        switch (index){
            case helmetIndex: return currentHelmet;
            case chestIndex: return currentChest;
            case legsIndex: return currentLegs;
            case bootsIndex: return currentBoots;
        }
        Debug.Log("Armor not selected");
        return null;
    }
    public int GetHelmetIndex() { return helmetIndex; }
    public int GetChestIndex() { return chestIndex; }
    public int GetLegsIndex() { return legsIndex; }
    public int GetBootsIndex() { return bootsIndex; }
    public Item SwapArmor(Item item, int index) {
        Item oldArmor = null;
        switch (index)
        {
            case helmetIndex:
                oldArmor = currentHelmet;
                currentHelmet = item;
                break;
            case chestIndex:
                oldArmor = currentChest;
                currentChest = item;
                break;
            case legsIndex:
                oldArmor = currentLegs;
                currentLegs = item;
                break;

            case bootsIndex:
                oldArmor = currentBoots;
                currentBoots = item;
                break;
        }
        return oldArmor;
    }

    public bool Equip(Item item, int index) {
        //Debug.Log(index);
        //Debug.Log(helmetIndex);
        switch (index)
        {
            case helmetIndex: 
                if (item.IsHelmet()) {
                    currentHelmet = item;
                    return true;
                }
                else {
                    //Debug.Log("Not a helmet");
                    return false;
                }
            case chestIndex:
                if (item.IsChest()) {
                    currentChest = item;
                    return true;
                }
                else {
                    //Debug.Log("Not a chest");
                    return false;
                }
            case legsIndex: 
                if (item.IsLegs()) {
                    currentLegs = item;
                    return true;
                }
                else {
                    //Debug.Log("Not a legs");
                    return false;
                }
            case bootsIndex:
                if (item.IsBoots()) {
                    currentBoots = item;
                    return true;
                }
                else {
                    //Debug.Log("Not a boots");
                    return false;
                }
            default:
                Debug.Log("Invalid equip");
                return false;
        }
    }

    public void Unequip(int index) {
        if (GetEquippedArmor(index) == null) { return; }
        switch (index)
        {
            case helmetIndex: ItemWorld.DropItem(player.GetComponent<Transform>(), camera.GetComponent<Transform>(), currentHelmet); return;
            case chestIndex: ItemWorld.DropItem(player.GetComponent<Transform>(), camera.GetComponent<Transform>(), currentChest); return;
            case legsIndex: ItemWorld.DropItem(player.GetComponent<Transform>(), camera.GetComponent<Transform>(), currentLegs); return;
            case bootsIndex: ItemWorld.DropItem(player.GetComponent<Transform>(), camera.GetComponent<Transform>(), currentBoots); return;
        }
        Debug.Log("Dropped armor");
    }

    public float bonusHP() {
        float totalBonusHP = 0f;
        if (currentHelmet != null) { totalBonusHP += 25f; }
        if (currentChest != null) { totalBonusHP += 25f; }
        if (currentLegs != null) { totalBonusHP += 25f; }
        if (currentBoots != null) { totalBonusHP += 25f; }
        return totalBonusHP;
    }
}
