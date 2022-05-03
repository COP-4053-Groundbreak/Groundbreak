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


    private PlayerConsumableInventory level1consumable = new PlayerConsumableInventory();
    private PlayerPassiveInventory level1passive = new PlayerPassiveInventory();
    private ActiveItem level1active = null;

    private void OnEnable()
    {
        /*        playerConsumableInventory = new PlayerConsumableInventory();
                playerPassiveInventory = new PlayerPassiveInventory();
                PlayerActiveItem = null;*/

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
    private void OnDestroy()
    {
/*        level1consumable = PlayerConsumableInventory;
        level1active = PlayerActiveItem;
        level1passive = PlayerPassiveInventory;*/
    }

    public void Plz()
    {
        PlayerConsumableInventory = level1consumable;
        PlayerActiveItem = level1active;
        PlayerPassiveInventory = level1passive;
    }
    public void SaveItems()
    {
        level1consumable = PlayerConsumableInventory;
        level1active = PlayerActiveItem;
        level1passive = PlayerPassiveInventory;
    }
}

