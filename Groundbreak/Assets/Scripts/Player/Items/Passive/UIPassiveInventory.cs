using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIPassiveInventory : MonoBehaviour
{
    public PlayerPassiveInventory passiveInventory;
    //ToolTipControler toolTipControler;
    // Position of the template for an item in the inventory,
    // and the empty game object to hold the clones of it
    Transform passiveItemContainer;
    Transform passiveItemTemplate;
    public HoldPlayerStats playerInformation;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    TurnLogic turnLogic;
    PlayerStats playerStats;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        turnLogic = FindObjectOfType<TurnLogic>();
        passiveItemContainer = transform.Find("passiveItemContainer");
        passiveItemTemplate = transform.Find("passiveItemTemplate");

    }

    // Set local reference of inventory and add it to the event system
    public void SetPassiveInventory(PlayerPassiveInventory passiveInventory, bool playerOrShop)
    {
        passiveInventory.UpdateItemList += Inventory_UpdateItemList;
        passiveInventory.AddItem(new PassiveItem { itemName = PassiveItem.PassiveItemName.AirPendant, amount = 1 }, playerStats);
        passiveInventory.AddItem(new PassiveItem { itemName = PassiveItem.PassiveItemName.AirPendant, amount = 1 }, playerStats);
        passiveInventory.AddItem(new PassiveItem { itemName = PassiveItem.PassiveItemName.AirPendant, amount = 1 }, playerStats);
        this.passiveInventory = passiveInventory;
        RefreshInventory();
    }
    // On the event, refresh the inventory visuals
    private void Inventory_UpdateItemList(object sender, EventArgs e)
    {

        RefreshInventory();
    }
    public void RefreshInventory()
    {
        // Refind itemContainer and itemTemplate
        passiveItemContainer = transform.Find("passiveItemContainer");
        passiveItemTemplate = transform.Find("passiveItemTemplate");

        // Delete all item icons except for the template
        foreach (Transform child in passiveItemContainer)
        {
            if (child == passiveItemTemplate)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);
            }

        }

        int xPos = 0;
        int yPos = 0;
        float itemSlotSize = 100f;
        // For each item in the inventory
        if (passiveInventory == null) 
        {
            return;
        }
        foreach (PassiveItem item in passiveInventory.GetItemList())
        {
            // Create new copy from itemTemplate under itemContainer
            RectTransform itemSlotRectTransform = Instantiate(passiveItemTemplate, passiveItemContainer).GetComponent<RectTransform>();
            // Sets object to be active, template which is copied is hidden
            itemSlotRectTransform.gameObject.SetActive(true);

            // On right click
            itemSlotRectTransform.GetComponent<PassiveItemClickable>().onRightClick = () =>
            {

            };

            // On left click
            itemSlotRectTransform.GetComponent<PassiveItemClickable>().onLeftClick = () =>
            {

            };

            // Get position of next item slot by multiplying its number by the size of the slot in pixels
            itemSlotRectTransform.anchoredPosition = new Vector2(xPos * itemSlotSize, yPos * itemSlotSize);

            itemSlotRectTransform.gameObject.GetComponent<PassiveItemClickable>().passiveItem = item;

            // Set sprite to item sprite
            Image spriteImage = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            spriteImage.sprite = item.GetSprite();
            // Set text displaying amount
            TextMeshProUGUI amountText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();

            if (item.amount > 1)
            {
                amountText.SetText(item.amount.ToString());
            }

            else
            {
                amountText.SetText("");
            }

            // Increment position in the inventory to the right
            xPos++;
            // if we are at the right edge, reset and move down a row
            if (xPos > 7)
            {
                xPos = 0;
                yPos--;
            }

        }
    }
    private void OnDestroy()
    {
        if (passiveInventory != null)
        {
            passiveInventory.UpdateItemList -= Inventory_UpdateItemList;
        }
    }
}
