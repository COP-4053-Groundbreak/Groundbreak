using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem
{
    public enum ItemName
    {
        smallHealthPotion,
        largeHealthPotion,
        smallSpeedPotion,
        largeSpeedPotion,
        smallStoneSkinPotion,
        largeStoneSkinPotion,
        smallStrengthPotion,
        largeStrengthPotion
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
            case ItemName.smallSpeedPotion:
                return ItemAssets.Instance.smallSpeedPotion;
            case ItemName.largeSpeedPotion:
                return ItemAssets.Instance.largeSpeedPotion;
            case ItemName.smallStoneSkinPotion:
                return ItemAssets.Instance.smallStoneSkinPotion;
            case ItemName.largeStoneSkinPotion:
                return ItemAssets.Instance.largeStoneSkinPotion;
            case ItemName.smallStrengthPotion:
                return ItemAssets.Instance.smallStrengthPotion;
            case ItemName.largeStrengthPotion:
                return ItemAssets.Instance.largeStrengthPotion;
        }

        return null;
    }

    public void ConsumeItem(PlayerStats playerStats)
    {
        switch (itemName)
        {

            case ItemName.smallHealthPotion:
                if (playerStats.GetHealth() + 30 < playerStats.GetMaxHealth())
                {
                    playerStats.ModifyHealth(30);
                }
                else 
                {
                    playerStats.MaxOutHealth();
                }
                break;
            case ItemName.largeHealthPotion:
                if (playerStats.GetHealth() + 60 < playerStats.GetMaxHealth())
                {
                    playerStats.ModifyHealth(60);
                }
                else
                {
                    playerStats.MaxOutHealth();
                }
                break;
            case ItemName.smallSpeedPotion:
                playerStats.ModifyMovementSpeed(10);
                break;
            case ItemName.largeSpeedPotion:
                playerStats.ModifyMovementSpeed(20);
                break;
            case ItemName.smallStoneSkinPotion:
                playerStats.ModifyArmor(4);
                break;
            case ItemName.largeStoneSkinPotion:
                playerStats.ModifyArmor(8);
                break;
            case ItemName.smallStrengthPotion:
                playerStats.ModifyThrowRange(1);
                break;
            case ItemName.largeStrengthPotion:
                playerStats.ModifyThrowRange(2);
                break;
        }
        if (playerStats.GetHealth() > playerStats.GetMaxHealth())
        {
            playerStats.MaxOutHealth();
        }
    }
    public string GetDescription()
    {
        switch (itemName)
        {
            case ItemName.smallHealthPotion:
                return "Small Health Potion\n +30Hp";
            case ItemName.largeHealthPotion:
                return "Large Health Potion\n +60Hp";
            case ItemName.smallSpeedPotion:
                return "Small Speed Potion\n +1 Movement";
            case ItemName.largeSpeedPotion:
                return "Large Speed Potion\n +2 Movement";
            case ItemName.smallStoneSkinPotion:
                return "Small StoneSkin Potion\n +4 Armor";
            case ItemName.largeStoneSkinPotion:
                return "Large StoneSkin Potion\n +8 Armor";
            case ItemName.smallStrengthPotion:
                return "Small Strength Potion\n +1 Throw Range";
            case ItemName.largeStrengthPotion:
                return "Large Strength Potion\n +2 Throw Range";
        }
        return null;
    }

    public bool hasDuration() 
    {
        if (itemName == ItemName.smallHealthPotion || itemName == ItemName.largeHealthPotion) 
        {
            return false;
        }
        return true;
    }

    public static ConsumableItem GetRandomConsumableItem() 
    {
        int itemIndex = Random.Range(0, 7);
        switch (itemIndex)
        {
            case 0:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.smallHealthPotion , amount = 1};
            case 1:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.largeHealthPotion, amount = 1 };
            case 2:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.smallSpeedPotion, amount = 1 };
            case 3:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.largeSpeedPotion, amount = 1 };
            case 4:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.smallStoneSkinPotion, amount = 1 };
            case 5:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.largeStoneSkinPotion, amount = 1 };
            case 6:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.smallStrengthPotion, amount = 1 };
            case 7:
                return new ConsumableItem { itemName = ConsumableItem.ItemName.largeStrengthPotion, amount = 1 };
        }
        return null;
    }
    public void UnConsumeItem(PlayerStats playerStats)
    {
        switch (itemName)
        {
            case ItemName.smallSpeedPotion:
                playerStats.ModifyMovementSpeed(-10);
                break;
            case ItemName.largeSpeedPotion:
                playerStats.ModifyMovementSpeed(-20);
                break;
            case ItemName.smallStoneSkinPotion:
                playerStats.ModifyArmor(-4);
                break;
            case ItemName.largeStoneSkinPotion:
                playerStats.ModifyArmor(-8);
                break;
            case ItemName.smallStrengthPotion:
                playerStats.ModifyThrowRange(-1);
                break;
            case ItemName.largeStrengthPotion:
                playerStats.ModifyThrowRange(-2);
                break;
        }
    }
}
