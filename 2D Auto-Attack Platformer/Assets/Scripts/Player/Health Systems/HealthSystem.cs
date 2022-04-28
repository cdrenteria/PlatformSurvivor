using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    const float MAX_HEALTH = 6;
    public float health;
    //Creates a new HealthSystem with a given amount of health
    public HealthSystem(float HealthAmount)
    {
        this.health = HealthAmount;
    }

    //Returns the health
    public float GetHealth()
    {
        return this.health;
    }

    //Reduces the health by the damage amount and updates the healthbar if given one
    public void damage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }

    }

    //Increases the health by the heal amount and updates the healthbar if given one
    public void heal(float healAmount)
    {
        health += healAmount;
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
    }

    public void increaseHealth(float increaseAmount)
    {
        if ( health + increaseAmount <= MAX_HEALTH)
        {
            health += increaseAmount;
        }
    }

    public void setHealth(float healthAmount)
    {
        health = healthAmount;
    }

    public void Die()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<IDamageable>().Die();
        } else
        {
            print("you died");
        }
    }
}
