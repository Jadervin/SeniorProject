using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterShockScript : MonoBehaviour
{
    [SerializeField] private int shockDamage = 1;
    [SerializeField] private bool hitWithCollider = false;
    [SerializeField] private float cooldownTimeMax = 1f;
    [SerializeField] private float timeSinceHit = 0f;
    public string ENEMYTAG = "Enemy";

    private void Update()
    {
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
