using System;

public class HealthSystem 
{
    public event EventHandler OnHealthChanged;
    private int health;
    private int healthMax;

    public HealthSystem(int healthMax){
        this.healthMax = healthMax;
        health = healthMax;

    }

    public int GetHealth(){
        return health;
    }

    public int getMaxHealth(){
        return healthMax;
    }

    public void SetHealth(int healthAmmount){
        health = healthAmmount;
    }

    public float GetHealthPercent(){
        return (float) health / healthMax;
    }

    public void Damage(int damageAmount){
        health -= damageAmount;
        if(health < 0){
            health = 0;
        }
        if(OnHealthChanged != null){
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount){
        health += healAmount;
        if(health > healthMax){
            health = healthMax;
        }
        if(OnHealthChanged != null){
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

}
