using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] int initiative = 3;

    // Starting variables to compare to
    public int startingHealth = 100;
    public int startingMovement = 20;
    public int startingArmor = 0;
    public int startingInitiative = 3;
    public float startingFire = 1.0f;
    public float startingWater = 1.0f;
    public float startingEarth = 1.0f;
    public float startingAir = 1.0f;

    public event EventHandler OnHealthChanged;
    public GameObject playerHealthBar;

    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        // Disabled so items can be added at the start
/*        maxHealth = startingHealth;
        currentHealth = startingHealth;
        movementPerTurn = startingMovement;
        armor = startingArmor;
        initiative = startingInitiative;
        fireMod = startingFire;
        waterMod = startingWater;
        earthMod = startingEarth;
        airMod = startingAir;*/

        // Create healthbar
        Transform healthBarTransform = Instantiate(playerHealthBar.transform, gameObject.transform);
        Vector3 healthBarLocalPosition = new Vector3(0, (float)1);
        healthBarTransform.localPosition = healthBarLocalPosition;

        PlayerHealthBar healthBar = healthBarTransform.GetComponent<PlayerHealthBar>();
        healthBar.Setup(this);
    }

    public void DealDamage(int damage) 
    {
        
        SoundManagerScript.PlaySound("playerdamage");
        currentHealth = currentHealth - (damage - armor);
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this, EventArgs.Empty);
        }
        if (currentHealth <= 0)
        {
            // Trigger Game over
            StartCoroutine(WaitAndDie());
        }
        else 
        {
            playerAnimator.SetTrigger("Hit");
        }

    }

    IEnumerator WaitAndDie() 
    {
        FindObjectOfType<PlayerHealthBar>().gameObject.SetActive(false);
        playerAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("Menu");
    }

    // Getters
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

    // Modifiers
    public void ModifyHealth(int value) 
    {
        currentHealth += value;
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }
    public void ModifyMaxHealth(int value)
    {
        maxHealth += value;
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this, EventArgs.Empty);
        }
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
        ReactionManager.MAGMA_DMG = (int)(ReactionManager.MAGMA_DMG * (fireMod * earthMod));
        ReactionManager.FIREBALL_DMG = (int)(ReactionManager.FIREBALL_DMG * (fireMod * airMod));
        // SMOKE DOESN'T DO DAMAGE
    }
    public void ModifyWaterMod(float value)
    {
        waterMod += value;
        ReactionManager.STORM_DMG = (int)(ReactionManager.STORM_DMG * (waterMod * airMod));
        // SMOKE AND MUD DO NO DAMAGE
    }
    public void ModifyEarthMod(float value)
    {
        earthMod += value;
        ReactionManager.MAGMA_DMG = (int)(ReactionManager.MAGMA_DMG * (fireMod * earthMod));
        ReactionManager.SANDSTORM_DMG = (int)(ReactionManager.SANDSTORM_DMG * (earthMod * airMod));
        // MUD DOESN'T DO DAMAGE
    }
    public void ModifyAirMod(float value)
    {
        airMod += value;
        ReactionManager.FIREBALL_DMG = (int)(ReactionManager.FIREBALL_DMG * (fireMod * airMod));
        ReactionManager.STORM_DMG = (int)(ReactionManager.STORM_DMG * (waterMod * airMod));
        ReactionManager.SANDSTORM_DMG = (int)(ReactionManager.SANDSTORM_DMG * (earthMod * airMod));
    }

    public void ModifyInitiative(int value)
    {
        initiative += value;
    }

    // Get health as a percent for healthbar
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    public void MaxOutHealth() 
    {
        currentHealth = maxHealth;
    }

    public void ModifyThrowRange(int value) 
    {
        FindObjectOfType<PlayerActions>().throwRange += value;
        if (FindObjectOfType<PlayerActions>().throwRange < 0) 
        {
            FindObjectOfType<PlayerActions>().throwRange = 0;
        }
    }

}
