using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] float pickUpTime = 1f;
    PlayerStats playerStats;
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void AddRandomItem(GameObject chest)
    {
        PassiveItem item = PassiveItem.GetRandomPassiveItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetEffect();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddItem(chest, item));

    }

    public void AddRandomConsumableItem(GameObject chest) 
    {
        ConsumableItem item = ConsumableItem.GetRandomConsumableItem();
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.GetSprite();
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = item.GetDescription();
        chest.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(WaitAndAddConsumableItem(chest, item));
    }


    // Adding items
    void EquipPassiveItem(PassiveItem item) 
    {
        switch (item.itemName)
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
        }
    }

    // Remove item
    void UnequipPassiveItem(PassiveItem item)
    {
        switch (item.itemName)
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
        }
    }

    // Enumerator to have a delay before item is added
    IEnumerator WaitAndAddItem(GameObject chest, PassiveItem item)
    {
        yield return new WaitForSeconds(pickUpTime);
        EquipPassiveItem(item);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator WaitAndAddConsumableItem(GameObject chest, ConsumableItem item)
    {
        yield return new WaitForSeconds(pickUpTime);
        FindObjectOfType<UIConsumableInventoryController>().uIInventory.consumableInventory.AddItem(item);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        chest.transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
        chest.transform.GetChild(2).gameObject.SetActive(false);
    }
}
