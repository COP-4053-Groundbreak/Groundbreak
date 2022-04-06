using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPassiveInventory 
{
    List<PassiveItem> itemList;
    public event EventHandler UpdateItemList;

    public PlayerPassiveInventory()
    {
        itemList = new List<PassiveItem>();
    }

    public void AddItem(PassiveItem item, PlayerStats playerStats)
    {
        
        bool itemFoundInInventory = false;
        foreach (PassiveItem inventoryItem in itemList)
        {
            if (inventoryItem.itemName == item.itemName)
            {
                itemFoundInInventory = true;
                inventoryItem.amount += item.amount;
            }
        }
        if (!itemFoundInInventory)
        {
            itemList.Add(item);
        };
        item.EquipPassiveItem(playerStats);
        UpdateItemList?.Invoke(this, EventArgs.Empty);
    }

    // Remove item from the inventory
    public void RemoveItem(PassiveItem item)
    {
        // Loop through each item in the list
        PassiveItem itemInInventory = null;
        foreach (PassiveItem inventoryItem in itemList)
        {
            // If item is found, decrease its amount
            if (inventoryItem.itemName == item.itemName)
            {
                itemInInventory = inventoryItem;
                inventoryItem.amount -= 1;
            }
        }
        // If amount is 0, remove it
        if (itemInInventory != null && itemInInventory.amount <= 0)
        {
            itemList.Remove(item);
        }

        // Update list using unity event manager
        UpdateItemList?.Invoke(this, EventArgs.Empty);

    }

    public List<PassiveItem> GetItemList()
    {
        return itemList;
    }
}
