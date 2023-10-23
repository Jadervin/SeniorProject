using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterShockScript : MonoBehaviour
{
    [SerializeField] private int shockDamage = 1;
    [SerializeField] private bool hitWithCollider = false;
    [SerializeField] private float cooldownTimeMax = 1f;
    [SerializeField] private float timeSinceHit = 0f;
    [SerializeField] private float timeSinceSpawned = 0f;
    [SerializeField] private float aftershockDespawnTime = 3f;
    public string ENEMYTAG = "Enemy";

    private void Update()
    {
        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned > aftershockDespawnTime)
        {
            timeSinceSpawned = 0;
            Destroy(gameObject);
        }

        if (hitWithCollider == true)
        {
            timeSinceHit += Time.deltaTime;
            

            if (timeSinceHit > cooldownTimeMax)
            {
                timeSinceHit = 0;

                hitWithCollider = false;

            }

            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(ENEMYTAG) && hitWithCollider == false)
        {
            collision.gameObject.GetComponent<EnemyScript>().DamageHealth(shockDamage);
            hitWithCollider = true;
        }
    }

}
