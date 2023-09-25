using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : EntityScript
{
    [SerializeField] private string ENEMYTAG = "Enemy";
    [SerializeField] private string ENEMY_PROJECTILE_TAG = "EnemyProjectile";

    [SerializeField] private float damageTimeBuffer = 1f;
    [SerializeField] private bool isInvincible;

    public event EventHandler<OnKnockbackEventArgs> OnPlayerKnockbackAction;
    public class OnKnockbackEventArgs : EventArgs
    {
        public GameObject collidedGameObject;
    }

    public event EventHandler OnPlayerHit;

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
        OnPlayerHit?.Invoke(this, EventArgs.Empty);


        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerHit?.Invoke(this, EventArgs.Empty);
            OnDeath();
        }
        else
        {
            StartCoroutine(playerInvincibility());
        }
    }

    public override void OnDeath()
    {
        this.gameObject.SetActive(false);
        MovementLimiter.instance.OnDeathManager();
        //Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if ((collision.gameObject.CompareTag(ENEMYTAG) && isInvincible == false))
        {
            int damage = collision.gameObject.GetComponent<EnemyScript>().GetDamage();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag(ENEMY_PROJECTILE_TAG) && isInvincible == false))
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

    IEnumerator playerInvincibility()
    {
        isInvincible = true;
        //ShieldIcon.SetActive(true);
        yield return new WaitForSeconds(damageTimeBuffer);
        isInvincible = false;
        //ShieldIcon.SetActive(false);
    }

}
