using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem 
{
    public enum ItemName
    {
        smallHealthPotion,
        largeHealthPotion
    }

    public ItemName itemName;
    public int amount;

    public Sprite GetSprite() 
    {
        switch (itemName) 
        {
            case ItemName.smallHealthPotion:
                return ItemAssets.Instance.smallHealthPotion;
            case ItemName.largeHealthPotion:
                return ItemAssets.Instance.largeHealthPotion;
        }

        return null;
    }

    public void ConsumeItem(PlayerStats playerStats) 
    {
        switch (itemName)
        {

            case ItemName.smallHealthPotion:
                playerStats.ModifyHealth(playerStats.GetHealth() + 30);
                break;
            case ItemName.largeHealthPotion:
                playerStats.ModifyHealth(playerStats.GetHealth() + 60);
                break;
        }
        if (playerStats.GetHealth() > playerStats.GetMaxHealth()) 
        {
            playerStats.MaxOutHealth();
        }
    }

}
