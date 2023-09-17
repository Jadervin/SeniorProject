using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityScript : MonoBehaviour
{
    [Header("Health Attributes")]
    /*
     * [SerializeField] 
     */
    protected int currentHealth;
    [SerializeField] protected int maxHealth;

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealHealth(int healAmount)
    {
        currentHealth += healAmount;
    }
    public void DamageHealth(int damageAmount) 
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    

    public void IncreaseMaxHealth(int hpIncreaseAmount)
    {
        maxHealth += hpIncreaseAmount;
        currentHealth = maxHealth;
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