using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : EntityScript
{
    [SerializeField] private string ENEMYTAG = "Enemy";
    public string BOSSENEMYTAG = "Boss Enemy";
    [SerializeField] private string ENEMY_PROJECTILE_TAG = "EnemyProjectile";
    //[SerializeField] private string HEALTHUPGRADETAG = "HealthUpgrade";

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        MovementLimiter.instance.OnDeathManager();

        StartCoroutine(DeathTimerUntilLoseScreen());
        //Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag(ENEMYTAG) && isInvincible == false || collision.gameObject.CompareTag(BOSSENEMYTAG) && isInvincible == false)
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
        if (collision.gameObject.CompareTag(ENEMY_PROJECTILE_TAG) && isInvincible == false && MovementLimiter.instance.playerActionStates != PlayerActionStates.COUNTERING /*&& is not tongue countering*/)
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

}
