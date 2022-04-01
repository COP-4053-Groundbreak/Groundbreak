using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem 
{
    public enum ItemName
    {
        test,
        test2
    }

    public ItemName itemName;
    public int amount;

    public Sprite GetSprite() 
    {
        switch (itemName) 
        {
            case ItemName.test:
                return ItemAssets.Instance.lightShieldSprite;
            case ItemName.test2:
                return ItemAssets.Instance.airPendantSprite;
        }

        return null;
    }

}
