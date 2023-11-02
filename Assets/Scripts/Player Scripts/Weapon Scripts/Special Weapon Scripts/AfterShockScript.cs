using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterShockScript : MonoBehaviour
{
    [SerializeField] private int shockDamage = 1;
    [SerializeField] private bool hitWithCollider = false;
    [SerializeField] private float timeSinceHit = 0f;
    [SerializeField] private float timeSinceSpawned = 0f;
    [SerializeField] private float shockCooldownTime = .2f;
    [SerializeField] private float shockDespawnTime = 3f;
    [SerializeField] private float shockRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned > shockDespawnTime)
        {
            timeSinceSpawned = 0;
            Destroy(gameObject);
        }


        timeSinceHit += Time.deltaTime;

        if (timeSinceHit > shockCooldownTime)
        {
            timeSinceHit = 0;

            hitWithCollider = false;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, shockRadius, enemyLayer);
            foreach (Collider2D collider in collider2Ds)
            {
                //collider.gameObject.GetComponent<EnemyScript>().DamageHealth(shockDamage);
                if(collider.gameObject.TryGetComponent<EnemyScript>(out EnemyScript enemy))
                {
                    enemy.DamageHealth(shockDamage);

                }
                else if(collider.gameObject.TryGetComponent<BossEnemyScript>(out BossEnemyScript boss))
                {
                    boss.DamageHealth(shockDamage);

                }


                hitWithCollider = true;
            }


        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shockRadius);
    }



}
