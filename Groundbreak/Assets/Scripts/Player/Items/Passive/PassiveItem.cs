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
        InitiativeWand,
        Cheese,
        MercuryBoots,
        HeavyShield,
        FireStaff,
        WaterStaff,
        EarthStaff,
        AirStaff,
        WildWand
    }

    public PassiveItemName itemName;
    public int amount;
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
            case PassiveItemName.Cheese:
                return ItemAssets.Instance.cheeseSprite;
            case PassiveItemName.MercuryBoots:
                return ItemAssets.Instance.mercuryBootsSprite;
            case PassiveItemName.HeavyShield:
                return ItemAssets.Instance.heavyShieldSprite;
            case PassiveItemName.FireStaff:
                return ItemAssets.Instance.fireStaffSprite;
            case PassiveItemName.WaterStaff:
                return ItemAssets.Instance.waterStaffSprite;
            case PassiveItemName.EarthStaff:
                return ItemAssets.Instance.earthStaffSprite;
            case PassiveItemName.AirStaff:
                return ItemAssets.Instance.airStaffSprite;
            case PassiveItemName.WildWand:
                return ItemAssets.Instance.wildWandSprite;
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
            case PassiveItemName.Cheese:
                return "Cheese? \n+75 HP";
            case PassiveItemName.MercuryBoots:
                return "Mercury Boots \n+2 Movement Range";
            case PassiveItemName.HeavyShield:
                return "Heavy Shield \n+5 Armor";
            case PassiveItemName.FireStaff:
                return "Fire Staff \n+25% Fire Strength";
            case PassiveItemName.WaterStaff:
                return "Water Staff \n+25% Water Strength";
            case PassiveItemName.EarthStaff:
                return "Earth Staff \n+25% Earth Strength";
            case PassiveItemName.AirStaff:
                return "Air Staff \n+25% Air Strength";
            case PassiveItemName.WildWand:
                return "Wild Wand \n+4 Initiative";
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

    // Get a random item
    public static PassiveItem GetRandomHighPassiveItem()
    {
        int itemIndex = Random.Range(0, 7);
        switch (itemIndex)
        {
            case 0:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.Cheese };
            case 1:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.MercuryBoots };
            case 2:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.HeavyShield };
            case 3:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.FireStaff };
            case 4:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.WaterStaff };
            case 5:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.EarthStaff };
            case 6:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.AirStaff };
            case 7:
                return new PassiveItem { itemName = PassiveItem.PassiveItemName.WildWand };
            default:
                return null;
        }
    }



    public void EquipPassiveItem(PlayerStats playerStats)
    {
        switch (itemName)
        {
            case PassiveItem.PassiveItemName.HealthRing:
                playerStats.ModifyHealth(50);
                playerStats.ModifyMaxHealth(50);
                break;
            case PassiveItem.PassiveItemName.SpeedBoots:
                playerStats.ModifyMovementSpeed(10);
                break;
            case PassiveItem.PassiveItemName.LightShield:
                playerStats.ModifyArmor(1);
                break;
            case PassiveItem.PassiveItemName.FirePendant:
                playerStats.ModifyFireMod(0.25f);
                break;
            case PassiveItem.PassiveItemName.WaterPendant:
                playerStats.ModifyWaterMod(0.25f);
                break;
            case PassiveItem.PassiveItemName.EarthPendant:
                playerStats.ModifyEarthMod(0.25f);
                break;
            case PassiveItem.PassiveItemName.AirPendant:
                
                playerStats.ModifyAirMod(0.25f);
                break;
            case PassiveItem.PassiveItemName.InitiativeWand:
                playerStats.ModifyInitiative(2);
                break;
            case PassiveItem.PassiveItemName.Cheese:
                playerStats.ModifyHealth(75);
                playerStats.ModifyMaxHealth(75);
                break;
            case PassiveItem.PassiveItemName.MercuryBoots:
                playerStats.ModifyMovementSpeed(20);
                break;
            case PassiveItem.PassiveItemName.HeavyShield:
                playerStats.ModifyArmor(5);
                break;
            case PassiveItem.PassiveItemName.FireStaff:
                playerStats.ModifyFireMod(0.5f);
                break;
            case PassiveItem.PassiveItemName.WaterStaff:
                playerStats.ModifyWaterMod(0.5f);
                break;
            case PassiveItem.PassiveItemName.EarthStaff:
                playerStats.ModifyEarthMod(0.5f);
                break;
            case PassiveItem.PassiveItemName.AirStaff:

                playerStats.ModifyAirMod(0.5f);
                break;
            case PassiveItem.PassiveItemName.WildWand:
                playerStats.ModifyInitiative(4);
                break;
        }
    }

    public void UnequipPassiveItem(PlayerStats playerStats)
    {
        switch (itemName)
        {
            case PassiveItem.PassiveItemName.HealthRing:
                playerStats.ModifyHealth(-50);
                playerStats.ModifyMaxHealth(-50);
                break;
            case PassiveItem.PassiveItemName.SpeedBoots:
                playerStats.ModifyMovementSpeed(-10);
                break;
            case PassiveItem.PassiveItemName.LightShield:
                playerStats.ModifyArmor(-1);
                break;
            case PassiveItem.PassiveItemName.FirePendant:
                playerStats.ModifyFireMod(-0.25f);
                break;
            case PassiveItem.PassiveItemName.WaterPendant:
                playerStats.ModifyWaterMod(-0.25f);
                break;
            case PassiveItem.PassiveItemName.EarthPendant:
                playerStats.ModifyEarthMod(-0.25f);
                break;
            case PassiveItem.PassiveItemName.AirPendant:
                playerStats.ModifyAirMod(-0.25f);
                break;
            case PassiveItem.PassiveItemName.InitiativeWand:
                playerStats.ModifyInitiative(-2);
                break;
            case PassiveItem.PassiveItemName.Cheese:
                playerStats.ModifyHealth(-75);
                playerStats.ModifyMaxHealth(-75);
                break;
            case PassiveItem.PassiveItemName.MercuryBoots:
                playerStats.ModifyMovementSpeed(-20);
                break;
            case PassiveItem.PassiveItemName.HeavyShield:
                playerStats.ModifyArmor(-5);
                break;
            case PassiveItem.PassiveItemName.FireStaff:
                playerStats.ModifyFireMod(-0.5f);
                break;
            case PassiveItem.PassiveItemName.WaterStaff:
                playerStats.ModifyWaterMod(-0.5f);
                break;
            case PassiveItem.PassiveItemName.EarthStaff:
                playerStats.ModifyEarthMod(-0.5f);
                break;
            case PassiveItem.PassiveItemName.AirStaff:

                playerStats.ModifyAirMod(-0.5f);
                break;
            case PassiveItem.PassiveItemName.WildWand:
                playerStats.ModifyInitiative(-4);
                break;
        }
    }
}
