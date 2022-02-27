using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] float pickUpTime = 1f;
    [SerializeField] Sprite HealthRingSprite;
    [SerializeField] Sprite SpeedBootsSprite;
    [SerializeField] Sprite LightShieldSprite;
    [SerializeField] Sprite FirePendantSprite;
    [SerializeField] Sprite WaterPendantSprite;
    [SerializeField] Sprite EarthPendantSprite;
    [SerializeField] Sprite WindPendantSprite;
    [SerializeField] Sprite InitiativeRingSprite;

    PlayerStats playerStats;
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void AddRandomItem(GameObject chest) 
    {
        StartCoroutine(WaitAndAddItem(chest));
        EquipHealthRing();
    }

    public void EquipHealthRing() 
    {
        playerStats.ModifyHealth(50);
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
    public void EquipInitiativeRing()
    {
        playerStats.ModifyInitiative(2);
    }

    IEnumerator WaitAndAddItem(GameObject chest)
    {
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = HealthRingSprite;
        yield return new WaitForSeconds(pickUpTime);
        chest.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }
}
