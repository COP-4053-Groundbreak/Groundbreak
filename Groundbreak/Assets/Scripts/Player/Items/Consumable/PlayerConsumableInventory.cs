using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumableInventory
{
    List<ConsumableItem> itemList;
    public event EventHandler UpdateItemList;

    public PlayerConsumableInventory()
    {
        itemList = new List<ConsumableItem>();
    }


    public void AddItem(ConsumableItem item)
    {
        bool itemFoundInInventory = false;
        foreach (ConsumableItem inventoryItem in itemList)
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
        }
        UpdateItemList?.Invoke(this, EventArgs.Empty);
    }

    // Remove item from the inventory
    public void RemoveItem(ConsumableItem item)
    {
        // Loop through each item in the list
        ConsumableItem itemInInventory = null;
        foreach (ConsumableItem inventoryItem in itemList)
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

    public List<ConsumableItem> GetItemList()
    {
        return itemList;
    }

}
