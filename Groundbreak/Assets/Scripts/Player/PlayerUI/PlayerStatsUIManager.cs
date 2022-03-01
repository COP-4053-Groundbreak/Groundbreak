using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerStatsUIManager : MonoBehaviour
{
    [SerializeField] GameObject healthText;
    [SerializeField] GameObject armorText;
    [SerializeField] GameObject speedText;
    [SerializeField] GameObject initiativeText;
    [SerializeField] GameObject fireText;
    [SerializeField] GameObject waterText;
    [SerializeField] GameObject earthText;
    [SerializeField] GameObject airText;

    [SerializeField] GameObject uiStats;

    PlayerStats playerStats;
    
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // This doesnt need to be done every frame but I doubt it will impact performance and im not going to bother with an event listener unless it does
    private void Update()
    {
        DisplayStats();
        ColorStats();
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (uiStats.activeInHierarchy)
            {
                uiStats.SetActive(false);
            }
            else
            {
                uiStats.SetActive(true);
            }
        }
    }

    private void DisplayStats()
    {
        healthText.GetComponent<TextMeshProUGUI>().text = playerStats.GetHealth().ToString() + "/" + playerStats.GetMaxHealth().ToString();
        armorText.GetComponent<TextMeshProUGUI>().text = playerStats.GetArmor().ToString();
        speedText.GetComponent<TextMeshProUGUI>().text = playerStats.GetMovementPerTurn().ToString();
        initiativeText.GetComponent<TextMeshProUGUI>().text = playerStats.GetInitiative().ToString();

        fireText.GetComponent<TextMeshProUGUI>().text = (playerStats.GetFireMod() * 100).ToString() + "%";
        waterText.GetComponent<TextMeshProUGUI>().text = (playerStats.GetFireMod() * 100).ToString() + "%";
        earthText.GetComponent<TextMeshProUGUI>().text = (playerStats.GetFireMod() * 100).ToString() + "%";
        airText.GetComponent<TextMeshProUGUI>().text = (playerStats.GetFireMod() * 100).ToString() + "%";
    }

    private void ColorStats() 
    {
        // Color Health
        if (playerStats.GetHealth() > playerStats.startingHealth)
        {
            healthText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetHealth() < playerStats.startingHealth) 
        {
            healthText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else 
        {
            healthText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color speed
        if (playerStats.GetMovementPerTurn() > playerStats.startingMovement)
        {
            speedText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetMovementPerTurn() < playerStats.startingMovement)
        {
            speedText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            speedText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color Armor
        if (playerStats.GetArmor() > playerStats.startingArmor)
        {
            armorText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetArmor() < playerStats.startingArmor)
        {
            armorText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            armorText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color init
        if (playerStats.GetInitiative() > playerStats.startingInitiative)
        {
            initiativeText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetInitiative() < playerStats.startingInitiative)
        {
            initiativeText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            initiativeText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color Fire Mod
        if (playerStats.GetFireMod() > playerStats.startingFire)
        {
            fireText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetFireMod() < playerStats.startingFire)
        {
            fireText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            fireText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color Water Mod
        if (playerStats.GetWaterMod() > playerStats.startingWater)
        {
            waterText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetWaterMod() < playerStats.startingWater)
        {
            waterText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            waterText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color Earth Mod
        if (playerStats.GetEarthMod() > playerStats.startingEarth)
        {
            earthText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetEarthMod() < playerStats.startingEarth)
        {
            earthText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            earthText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        // Color Air Mod
        if (playerStats.GetAirMod() > playerStats.startingAir)
        {
            airText.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if (playerStats.GetAirMod() < playerStats.startingAir)
        {
            airText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            airText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }
}
