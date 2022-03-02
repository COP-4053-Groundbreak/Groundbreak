using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    PlayerStats playerStats;
    
    public void Setup(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        playerStats.OnHealthChanged += healthSystem_OnHealthChanged;
    }

    private void healthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("Bar").localScale = new Vector3(playerStats.GetHealthPercent(), 2);
    }

    private void Update()
    {
        // transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 2);
    }
}
