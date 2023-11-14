using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : EntityScript
{
   

    [SerializeField] private float damageTimeBuffer = 1f;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float deathTimer = 2f;
    

    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    public event EventHandler<OnKnockbackEventArgs> OnPlayerKnockbackAction;
    public class OnKnockbackEventArgs : EventArgs
    {
        public GameObject collidedGameObject;
    }

    public event EventHandler OnPlayerHealthChanged;
    public event EventHandler OnPlayerMaxHealthChanged;

    public event EventHandler OnPlayerDamaged;
    public event EventHandler OnPlayerDead;

   /*
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
*/
    public override void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;
        OnPlayerHealthChanged?.Invoke(this, EventArgs.Empty);
        

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerHealthChanged?.Invoke(this, EventArgs.Empty);
            OnDeath();
        }
        else
        {
            OnPlayerDamaged?.Invoke(this, EventArgs.Empty);
            StartCoroutine(playerInvincibility());
        }
    }

    public void IncreaseMaxHealth(int hpIncreaseAmount)
    {
        maxHealth += hpIncreaseAmount;
        currentHealth = maxHealth;

        OnPlayerMaxHealthChanged?.Invoke(this, EventArgs.Empty);
    }


    public void HealthRefill()
    {
        currentHealth = maxHealth;

        OnPlayerHealthChanged?.Invoke(this, EventArgs.Empty);


    }

    public override void OnDeath()
    {
        foreach(var sprite in sprites)
        {
            sprite.enabled = false;
        }
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
        MovementLimiter.instance.OnDeathManager();

        StartCoroutine(DeathTimerUntilLoseScreen());
        //Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG) && isInvincible == false || collision.gameObject.CompareTag(TagReferencesScript.BOSSENEMYTAG) && isInvincible == false)
        {
            int damage = collision.gameObject.GetComponent<EntityScript>().GetEnemyDamage();
            DamageHealth(damage);

            if (currentHealth > 0)
            {
                //player gets knocked back
                OnPlayerKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
                {
                    collidedGameObject = collision.gameObject
                });
            }
        }
        /*
        if(collision.gameObject.CompareTag(HEALTHUPGRADETAG))
        {
            collision.gameObject.SetActive(false);
            IncreaseMaxHealth(healthUpgradeIncrease);

            Destroy(collision.gameObject);
        }*/
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.ENEMY_PROJECTILE_TAG) && isInvincible == false && MovementLimiter.instance.playerActionStates != PlayerActionStates.COUNTERING /*&& is not tongue countering*/)
        {
            int damage = collision.gameObject.GetComponent<EnemyProjectileScript>().GetDamage();
            DamageHealth(damage);

            if (currentHealth > 0)
            {
                //player gets knocked back
                OnPlayerKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
                {
                    collidedGameObject = collision.gameObject
                });
            }
        }
    }

    private IEnumerator playerInvincibility()
    {
        isInvincible = true;
        //ShieldIcon.SetActive(true);
        yield return new WaitForSeconds(damageTimeBuffer);
        isInvincible = false;
        //ShieldIcon.SetActive(false);
    }

    private IEnumerator DeathTimerUntilLoseScreen()
    {
        
        yield return new WaitForSeconds(deathTimer);
        Loader.Load(Loader.GameScenes.LoseScene);
        
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }
}
