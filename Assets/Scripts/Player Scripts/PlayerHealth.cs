using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityScript
{
    [SerializeField] private string ENEMYTAG = "Enemy";

    [SerializeField] private float damageTimeBuffer = 1f;
    [SerializeField] private bool isInvincible;

    public event EventHandler<OnKnockbackEventArgs> OnPlayerKnockbackAction;

    public class OnKnockbackEventArgs : EventArgs
    {
        public GameObject collidedGameObject;
    }

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

        if (currentHealth <= 0)
        {
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
      
        if (collision.gameObject.CompareTag(ENEMYTAG) && isInvincible == false)
        {
            int damage = collision.gameObject.GetComponent<EnemyScript>().GetDamage();
            DamageHealth(damage);

            //player gets knocked back
            OnPlayerKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
            {
                collidedGameObject = collision.gameObject
            });
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
