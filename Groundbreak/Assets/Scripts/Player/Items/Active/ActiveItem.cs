using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem
{

    public enum ActiveItemName
    {
        Sword,
        Bow,
        BlinkRune,
        FireballScroll,
        RepulsionWand,
        AttractionWand

    }

    public ActiveItemName itemName;


    public Sprite GetSprite() 
    {
        switch (itemName) 
        {
            case ActiveItemName.Sword:
                return ItemAssets.Instance.swordSprite;
            case ActiveItemName.Bow:
                return ItemAssets.Instance.bowSprite;
            case ActiveItemName.BlinkRune:
                return ItemAssets.Instance.blinkRuneSprite;
            case ActiveItemName.FireballScroll:
                return ItemAssets.Instance.fireballScrollSprite;
            case ActiveItemName.RepulsionWand:
                return ItemAssets.Instance.repulsionWandSprite;
            case ActiveItemName.AttractionWand:
                return ItemAssets.Instance.attractionWandSprite;
        }
        return null;
    }

    public static ActiveItem GetRandomActiveItem() 
    {
        int itemIndex = Random.Range(0, 5);
        switch (itemIndex) 
        {
            case 0:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.Sword };
            case 1:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.Bow };
            case 2:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.BlinkRune };
            case 3:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.FireballScroll };
            case 4:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.RepulsionWand };
            case 5:
                return new ActiveItem { itemName = ActiveItem.ActiveItemName.AttractionWand };
        }
        return null;
    }


    public string GetDescription() 
    {
        switch (itemName)
        {
            case ActiveItemName.Sword:
                return "A sword\nRange 1\nCooldown " + GetCooldown().ToString();
            case ActiveItemName.Bow:
                return "A bow\nRange 3\nCooldown " + GetCooldown().ToString();
            case ActiveItemName.BlinkRune:
                return "A teleport rune\nRange 3\nCooldown " + GetCooldown().ToString();
            case ActiveItemName.FireballScroll:
                return "A Fireball Scroll\nRange 3\nCooldown " + GetCooldown().ToString();
            case ActiveItemName.RepulsionWand:
                return "A Repulsion Wand\nRange 3\nCooldown " + GetCooldown().ToString();
            case ActiveItemName.AttractionWand:
                return "An Attraction Wand\nRange 3\nCooldown " + GetCooldown().ToString();
        }
        return "";
    }

    public int GetCooldown()
    {
        switch (itemName)
        {
            case ActiveItemName.Sword:
                return 1;
            case ActiveItemName.Bow:
                return 1;
            case ActiveItemName.BlinkRune:
                return 3;
            case ActiveItemName.FireballScroll:
                return 4;
            case ActiveItemName.RepulsionWand:
                return 2;
            case ActiveItemName.AttractionWand:
                return 2;
        }
        return -1;
    }

}
