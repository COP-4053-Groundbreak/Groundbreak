using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem {

    public enum PassiveItemName
    { 
        HealthRing,
        SpeedBoots,
        LightShield,
        FirePendant,
        WaterPendant,
        EarthPendant,
        AirPendant,
        InitiativeWand
    }

    public PassiveItemName itemName;

    public Sprite GetSprite() 
    {
        switch (itemName) 
        {
            case PassiveItemName.HealthRing:
                return ItemAssets.Instance.healthRingSprite;
            case PassiveItemName.SpeedBoots:
                return ItemAssets.Instance.speedBootsSprite;
            case PassiveItemName.LightShield:
                return ItemAssets.Instance.lightShieldSprite;
            case PassiveItemName.FirePendant:
                return ItemAssets.Instance.firePendantSprite;
            case PassiveItemName.WaterPendant:
                return ItemAssets.Instance.waterPendantSprite;
            case PassiveItemName.EarthPendant:
                return ItemAssets.Instance.earthPendantSprite;
            case PassiveItemName.AirPendant:
                return ItemAssets.Instance.airPendantSprite;
            case PassiveItemName.InitiativeWand:
                return ItemAssets.Instance.initiativeWandSprite;
        }
        return null;
    }

    public string GetEffect()
    {
        switch (itemName)
        {
            case PassiveItemName.HealthRing:
                return "Health Ring \n+50 HP";
            case PassiveItemName.SpeedBoots:
                return "Speed Boots \n+1 Movement Range";
            case PassiveItemName.LightShield:
                return "Light Shield \n+1 Armor";
            case PassiveItemName.FirePendant:
                return "Fire Pendant \n+25% Fire Strength";
            case PassiveItemName.WaterPendant:
                return "Water Pendant \n+25% Water Strength";
            case PassiveItemName.EarthPendant:
                return "Earth Pendant \n+25% Earth Strength";
            case PassiveItemName.AirPendant:
                return "Air Pendant \n+25% Air Strength";
            case PassiveItemName.InitiativeWand:
                return "Initative Wand \n+2 Initiative";
        }
        return null;
    }



    // Get a random item
    public static PassiveItem GetRandomPassiveItem()
    {
        int itemIndex = Random.Range(0, 7);
        switch (itemIndex)
        {
            case 0:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.HealthRing };
            case 1:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.SpeedBoots };
            case 2:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.LightShield };
            case 3:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.FirePendant };
            case 4:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.WaterPendant };
            case 5:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.EarthPendant };
            case 6:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.AirPendant };
            case 7:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.InitiativeWand };
            default:
                return null;
        }
    }

}
