using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object that holds
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats")]
public class HoldPlayerStats : ScriptableObject
{
    public PlayerConsumableInventory playerConsumableInventory = new PlayerConsumableInventory();

    private void OnEnable()
    {
        
    }
}

