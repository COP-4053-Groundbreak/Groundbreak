using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int currentHealth = 100;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int movementPerTurn = 20;
    // Armor stat so we can have items reduce damage
    [SerializeField] int armor = 0;
    // Modifiers to damage done by using certain tiles so we can have items that boost effects
    [SerializeField] float fireMod = 1.0f;
    [SerializeField] float waterMod = 1.0f;
    [SerializeField] float earthMod = 1.0f;
    [SerializeField] float airMod = 1.0f;
    // Inititive for who goes when during turns
    [SerializeField] int initiative = 5;

    public int startingHealth = 100;
    public int startingMovement = 20;
    public int startingArmor = 0;
    public int startingInitiative = 5;
    public float startingFire = 1.0f;
    public float startingWater = 1.0f;
    public float startingEarth = 1.0f;
    public float startingAir = 1.0f;

    private void Start()
    {
        maxHealth = startingHealth;
        currentHealth = startingHealth;
        movementPerTurn = startingMovement;
        armor = startingArmor;
        initiative = startingInitiative;
        fireMod = startingFire;
        waterMod = startingWater;
        earthMod = startingEarth;
        airMod = startingAir;
    }

    public void DealDamage(int damage) 
    {
        currentHealth = currentHealth - (damage - armor);
        if (currentHealth < 0) 
        {
            // Trigger Game over
        }
    }

    public int GetHealth() 
    {
        return currentHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetArmor()
    {
        return armor;
    }
    public int GetMovementPerTurn() 
    {
        return movementPerTurn;
    }
    public int GetInitiative()
    {
        return initiative;
    }
    public float GetWaterMod()
    {
        return waterMod;
    }
    public float GetEarthMod()
    {
        return earthMod;
    }
    public float GetFireMod()
    {
        return fireMod;
    }
    public float GetAirMod()
    {
        return airMod;
    }


    public void ModifyHealth(int value) 
    {
        currentHealth += value;
    }
    public void ModifyMaxHealth(int value)
    {
        maxHealth += value;
    }
    public void ModifyMovementSpeed(int value)
    {
        movementPerTurn += value;
    }
    public void ModifyArmor(int value)
    {
        armor += value;
    }
    public void ModifyFireMod(float value)
    {
        fireMod += value;
    }

    public void ModifyWaterMod(float value)
    {
        waterMod += value;
    }
    public void ModifyEarthMod(float value)
    {
        earthMod += value;
    }
    public void ModifyAirMod(float value)
    {
        airMod += value;
    }

    public void ModifyInitiative(int value)
    {
        initiative += value;
    }

}
