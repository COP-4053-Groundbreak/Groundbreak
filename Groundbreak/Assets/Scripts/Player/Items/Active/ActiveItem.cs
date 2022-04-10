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

}
