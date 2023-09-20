using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityScript
{
    [SerializeField] private string ENEMYTAG = "Enemy";

    [SerializeField] private float damageTimeBuffer = 1f;
    [SerializeField] private bool isInvincible;

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
        if (isInvincible == false)
        {
            if (collision.gameObject.CompareTag(ENEMYTAG))
            {
                int damage = collision.gameObject.GetComponent<EnemyScript>().GetDamage();
                DamageHealth(damage);
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
