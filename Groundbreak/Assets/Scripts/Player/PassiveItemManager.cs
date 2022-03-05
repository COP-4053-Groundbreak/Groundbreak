using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] float pickUpTime = 1f;
    // Sprites of items
    [SerializeField] Sprite HealthRingSprite;
    [SerializeField] Sprite SpeedBootsSprite;
    [SerializeField] Sprite LightShieldSprite;
    [SerializeField] Sprite FirePendantSprite;
    [SerializeField] Sprite WaterPendantSprite;
    [SerializeField] Sprite EarthPendantSprite;
    [SerializeField] Sprite WindPendantSprite;
    [SerializeField] Sprite InitiativeStaffSprite;
    PlayerStats playerStats;
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void AddRandomItem(GameObject chest)
    {
        int itemIndex = Random.Range(0, 7);
        switch (itemIndex)
        {
            case 0:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = HealthRingSprite;
                break;
            case 1:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = SpeedBootsSprite;
                break;
            case 2:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = LightShieldSprite;
                break;
            case 3:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = FirePendantSprite;
                break;
            case 4:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WaterPendantSprite;
                break;
            case 5:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = EarthPendantSprite;
                break;
            case 6:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WindPendantSprite;
                break;
            case 7:
                chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = InitiativeStaffSprite;
                break;
            default:
                break;
        }
        StartCoroutine(WaitAndAddItem(chest, itemIndex));

    }

    // Adding items
    public void EquipHealthRing()
    {
        playerStats.ModifyHealth(50);
        playerStats.ModifyMaxHealth(50);
    }

    public void EquipSpeedBoots()
    {
        playerStats.ModifyMovementSpeed(10);
    }

    public void EquipLightShield()
    {
        playerStats.ModifyArmor(1);
    }
    public void EquipFirePendant()
    {
        playerStats.ModifyFireMod(0.25f);
    }

    public void EquipWaterPendant()
    {
        playerStats.ModifyWaterMod(0.25f);
    }

    public void EquipEarthPendant()
    {
        playerStats.ModifyEarthMod(0.25f);
    }

    public void EquipWindPendant()
    {
        playerStats.ModifyFireMod(0.25f);
    }
    public void EquipInitiativeStaff()
    {
        playerStats.ModifyInitiative(2);
    }

    // Enumerator to have a delay before item is added
    IEnumerator WaitAndAddItem(GameObject chest, int itemIndex)
    {
        yield return new WaitForSeconds(pickUpTime);
        switch (itemIndex)
        {
            case 0:
                EquipHealthRing();
                break;
            case 1:
                EquipSpeedBoots();
                break;
            case 2:
                EquipLightShield();
                break;
            case 3:
                EquipFirePendant();
                break;
            case 4:
                EquipWaterPendant();
                break;
            case 5:
                EquipEarthPendant();
                break;
            case 6:
                EquipWindPendant();
                break;
            case 7:
                EquipInitiativeStaff();
                break;
            default:
                break;
        }

        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }
}
