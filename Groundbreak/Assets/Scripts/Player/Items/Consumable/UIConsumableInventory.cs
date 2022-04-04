using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIConsumableInventory : MonoBehaviour
{
    public PlayerConsumableInventory consumableInventory;
    //ToolTipControler toolTipControler;
    // Position of the template for an item in the inventory,
    // and the empty game object to hold the clones of it
    Transform itemContainer;
    Transform itemTemplate;
    public HoldPlayerStats playerInformation;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;
    TurnLogic turnLogic;

    private void Awake()
    {

        turnLogic = FindObjectOfType<TurnLogic>();
        itemContainer = transform.Find("itemContainer");
        itemTemplate = transform.Find("itemTemplate");

    }
    // Set local reference of inventory and add it to the event system
    public void SetConsumableInventory(PlayerConsumableInventory comsumableInventory, bool playerOrShop)
    {
        this.consumableInventory = comsumableInventory;
        comsumableInventory.UpdateItemList += Inventory_UpdateItemList;
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.smallHealthPotion, amount = 3 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.largeHealthPotion, amount = 2 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.smallSpeedPotion, amount = 3 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.largeSpeedPotion, amount = 2 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.smallStoneSkinPotion, amount = 3 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.largeStoneSkinPotion, amount = 2 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.smallStrengthPotion, amount = 3 });
        comsumableInventory.AddItem(new ConsumableItem { itemName = ConsumableItem.ItemName.largeStrengthPotion, amount = 2 });
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
        itemContainer = transform.Find("itemContainer");
        itemTemplate = transform.Find("itemTemplate");


        // Delete all item icons except for the template
        foreach (Transform child in itemContainer)
        {
            if (child == itemTemplate)
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
        foreach (ConsumableItem item in consumableInventory.GetItemList())
        {
            // Create new copy from itemTemplate under itemContainer
            RectTransform itemSlotRectTransform = Instantiate(itemTemplate, itemContainer).GetComponent<RectTransform>();
            // Sets object to be active, template which is copied is hidden
            itemSlotRectTransform.gameObject.SetActive(true);

            // On right click
            itemSlotRectTransform.GetComponent<ConsumableItemClickable>().onRightClick = () =>
            {

            };

            // On left click
            itemSlotRectTransform.GetComponent<ConsumableItemClickable>().onLeftClick = () =>
            {
                item.ConsumeItem(FindObjectOfType<PlayerStats>());
                consumableInventory.RemoveItem(item);
                // Update movement remaining so you get benefits this turn
                switch (item.itemName) 
                {
                    case ConsumableItem.ItemName.smallSpeedPotion:
                        FindObjectOfType<PlayerMovement>().currentMovementRemaining += 10;
                        break;
                    case ConsumableItem.ItemName.largeSpeedPotion:
                        FindObjectOfType<PlayerMovement>().currentMovementRemaining += 20;
                        break;
                }
                if (item.hasDuration()) 
                {
                    StartCoroutine(PotionDuration(item, turnLogic.turnCount));
                }
            };

            // Get position of next item slot by multiplying its number by the size of the slot in pixels
            itemSlotRectTransform.anchoredPosition = new Vector2(xPos * itemSlotSize, yPos * itemSlotSize);

            itemSlotRectTransform.gameObject.GetComponent<ConsumableItemClickable>().consumableItem = item;

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

    IEnumerator PotionDuration(ConsumableItem item, int startTurn) 
    {
        yield return new WaitUntil(() => Check(startTurn));
        item.UnConsumeItem(FindObjectOfType<PlayerStats>());
    }

    // Nessicary to unsubscribe from event to prevent broken references when reloading scene
    private void OnDestroy()
    {
        if (consumableInventory != null)
        {
            consumableInventory.UpdateItemList -= Inventory_UpdateItemList;
        }
    }

    private bool Check(int startTurn) 
    {
        if (turnLogic.turnCount == startTurn + 2 || !turnLogic.isCombatPhase) 
        {
            return true;
        }
        return false;
    }
}
