using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object that holds
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats")]
public class HoldPlayerStats : ScriptableObject
{
    private PlayerConsumableInventory PlayerConsumableInventory = new PlayerConsumableInventory();
    private PlayerPassiveInventory PlayerPassiveInventory = new PlayerPassiveInventory();
    private ActiveItem PlayerActiveItem = null;
    private void OnEnable()
    {
        playerConsumableInventory = new PlayerConsumableInventory();
        playerPassiveInventory = new PlayerPassiveInventory();
    }

    public PlayerConsumableInventory playerConsumableInventory
    {
        get { return PlayerConsumableInventory; }
        set { PlayerConsumableInventory = value; }
    }

    public PlayerPassiveInventory playerPassiveInventory
    {
        get { return PlayerPassiveInventory; }
        set { PlayerPassiveInventory = value; }
    }

    public ActiveItem playerActiveItem
    {
        get { return PlayerActiveItem; }
        set { PlayerActiveItem = value; }
    }
}

