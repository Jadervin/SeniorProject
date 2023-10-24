using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    [Header("Health Attributes")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;

    

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    

    public void HealHealth(int healAmount)
    {
        currentHealth += healAmount;
    }
    public virtual void DamageHealth(int damageAmount) 
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
        }
    }
    

    

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }


    public abstract void OnDeath();
    
}
