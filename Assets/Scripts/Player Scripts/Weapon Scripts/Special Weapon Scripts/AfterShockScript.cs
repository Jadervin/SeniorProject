using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterShockScript : MonoBehaviour
{
    [SerializeField] private int shockDamage = 1;
    [SerializeField] private bool hitWithCollider = false;
    //[SerializeField] private float timeSinceHit = 0f;
    //[SerializeField] private float timeSinceSpawned = 0f;
    [SerializeField] private float shockCooldownTime = .2f;
    [SerializeField] private float shockDespawnTime = 3f;


    private void Update()
    {
/*        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned > aftershockDespawnTime)
        {
            timeSinceSpawned = 0;
            Destroy(gameObject);
        }

        if (hitWithCollider == true)
        {
            timeSinceHit += Time.deltaTime;

            if (timeSinceHit > shockCooldownTime)
            {
                timeSinceHit = 0;

                hitWithCollider = false;

            }

        }*/


        if (hitWithCollider == true)
        {
            StartCoroutine(ShockCooldown());
        }


        StartCoroutine(SpawnCooldown());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG) && hitWithCollider == false || collision.gameObject.CompareTag(TagReferencesScript.BOSSENEMYTAG) && collision.gameObject.GetComponent<BossEnemyScript>().GetBossEnemyState() != BossEnemyStates.WAITINGFORPLAYER && hitWithCollider == false)
        {
            collision.gameObject.GetComponent<EnemyScript>().DamageHealth(shockDamage);
            hitWithCollider = true;
        }
    }

    public IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(shockDespawnTime);

        Destroy(gameObject);
    }

    public IEnumerator ShockCooldown()
    {
        yield return new WaitForSeconds(shockCooldownTime);

        hitWithCollider = false;
    }



}
