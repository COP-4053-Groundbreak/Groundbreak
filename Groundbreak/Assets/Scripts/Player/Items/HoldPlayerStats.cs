using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object that holds
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats")]
public class HoldPlayerStats : ScriptableObject
{
    public PlayerConsumableInventory playerConsumableInventory = new PlayerConsumableInventory();
    public PlayerPassiveInventory playerPassiveInventory = new PlayerPassiveInventory();
    public ActiveItem playerActiveItem = null;
    private void OnEnable()
    {
        playerConsumableInventory = new PlayerConsumableInventory();
        playerPassiveInventory = new PlayerPassiveInventory();
        playerActiveItem = null;
    }

}

