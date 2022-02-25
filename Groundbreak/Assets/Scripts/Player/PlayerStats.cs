using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] int health = 100;
    [SerializeField] int movementPerTurn = 20;
    // Armor stat so we can have items reduce damage
    [SerializeField] int armor = 0;
    // Modifiers to damage done by using certain tiles so we can have items that boost effects
    [SerializeField] float fireMod = 1.0f;
    [SerializeField] float waterMod = 1.0f;
    [SerializeField] float earthMod = 1.0f;
    [SerializeField] float windMod = 1.0f;
    // Inititive for who goes when during turns
    [SerializeField] int initiative = 5;

    public void DealDamage(int damage) 
    {
        health = health - (damage - armor);
        if (health < 0) 
        {
            // Trigger Game over
        }
    }
    public int GetMovementPerTurn() 
    {
        return movementPerTurn;
    }

}
