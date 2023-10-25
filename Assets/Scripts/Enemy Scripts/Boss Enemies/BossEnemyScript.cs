using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossEnemyStates
{
    WAITINGFORPLAYER,
    IDLE,
    WALKING,
    ATTACKING,
    STUNNED,
    DEATH
}

public enum BossEnemyAttackStates
{
    NONE,
    SHOOT,
    DASH
}



public class BossEnemyScript : EntityScript
{
    [Header("States")]
    [SerializeField] protected BossEnemyStates bossEnemyState;
    [SerializeField] protected BossEnemyAttackStates bossAttackState;
    [SerializeField] protected AttackCounterStates attackCounterState;


    [Header("Components")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D polyCollider;
    [SerializeField] protected GameObject enemyParent;
    [SerializeField] protected Transform playerTarget;
    [SerializeField] protected SpriteRenderer mainSprite;
    [SerializeField] protected GameObject itemToSpawn;
    protected Color mainColor;

    [Header("Booleans")]
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool currentlyAttacking;
    [SerializeField] protected bool stunTimerOn;
    [SerializeField] protected bool notStunnedAnymore;

    
/*
    [Header("Tags")]
    public string WALLTAG = "Wall";
    public string TONGUE_COUNTER_TAG = "TongueCounter";
*/

    [Header("Layer Masks")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Waiting for Player State Variables")]
    [SerializeField] protected bool canSeePlayer;
    [SerializeField] protected float playerTriggerRadius = 1.5f;
    [SerializeField] protected List<GameObject> wallsToSpawn = new List<GameObject>();

    [Header("Idle State Variables")]
    [SerializeField] protected float idleWaitTime = 3f;

    [Header("Attack State Variables")]
    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected bool isUsingAttack = false;
    [SerializeField] protected bool isRecharging = false;
    [SerializeField] protected float rechargeTime = 3f;


    [Header("Shooting Variables")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] protected float shootReloadTime = 2.0f;
    [SerializeField] protected int numOfShots = 3;

    [Header("Dash Variables")]
    [SerializeField] protected float dashAttackChargeTime = 5f;
    [SerializeField] protected float dashAttackRechargeTime = 1f;

    
    [SerializeField] protected float dashPower = 24.0f;
    [SerializeField] protected float attackTime = .4f;
    [SerializeField] protected float counterableTimeFrame = .3f;

    [SerializeField] protected bool isChargingAttack = false;
    


    [Header("Knockback Variables")]
    [SerializeField] protected float knockbackStrength = 16;
    [SerializeField] protected float delayTime = .3f;
    [SerializeField] protected float stunTime = 2f;

    private void Start()
    {
        playerTarget = FindAnyObjectByType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();
        polyCollider = GetComponent<Collider2D>();

        counterableTimeFrame = attackTime;

        mainColor = mainSprite.color;

        foreach (GameObject wall in wallsToSpawn)
        {
            wall.SetActive(false);
        }

    }


    private void Update()
    {
        switch (bossEnemyState)
        {
            case BossEnemyStates.WAITINGFORPLAYER:
                mainSprite.color = Color.gray;
                CheckForPlayer();
                break;

            case BossEnemyStates.IDLE:
                StartCoroutine(IdleTimer());
                break;

            case BossEnemyStates.WALKING:
                
                break;

            case BossEnemyStates.ATTACKING:
                
                if(canAttack == true)
                {
                    canAttack = false;
                    StartCoroutine(Shooting());
                }


                break;

            case BossEnemyStates.STUNNED:
                
                break;

            case BossEnemyStates.DEATH:
                
                break;

            default:
                break;

        }
    }

    private void CheckForPlayer()
    {
        canSeePlayer = Physics2D.OverlapCircle(transform.position, playerTriggerRadius, playerLayer);


        if (canSeePlayer == true)
        {
            playerTriggerRadius = 0;
            mainSprite.color = mainColor;

            foreach(GameObject wall in wallsToSpawn)
            {
                wall.SetActive(true);
            }

            bossEnemyState = BossEnemyStates.IDLE;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        
        //Drawing the Attack Trigger Circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerTriggerRadius);

    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(idleWaitTime);
        bossEnemyState = BossEnemyStates.ATTACKING;
    }


    private IEnumerator Shooting()
    {
        isUsingAttack = true;

        for(int i = 0; i<numOfShots; i++)
        {
            GameObject temp = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);
            //Debug.Log("Spawned Bullet");
            yield return new WaitForSeconds(shootReloadTime);
        }

        yield return new WaitForSeconds(rechargeTime);

        canAttack = true;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.TONGUE_COUNTER_TAG) && bossEnemyState == BossEnemyStates.ATTACKING && attackCounterState == AttackCounterStates.COUNTERABLE)
        {
            /*
            Knockback enemy
            OnEnemyKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
            {
                collidedGameObject = collision.gameObject
            });
            */
            //StopCoroutine(EnemyAttack());

            EnemyKnockbackAction(collision.gameObject);

            //enemyState = EnemyStates.STUNNED;
        }
    }

    protected void EnemyKnockbackAction(GameObject collidedGameObject)
    {
/*        StopAllCoroutines();

        Debug.Log(transform.position);
        Debug.Log(collidedGameObject.transform.position);

        Debug.Log(transform.position - collidedGameObject.transform.position);

        StopAllCoroutines();*/
        Vector2 knockbackDirection = (transform.position - collidedGameObject.transform.position).normalized;
        knockbackDirection.y *= -1;
        Debug.Log(knockbackDirection * knockbackStrength);

        rb.velocity = Vector2.zero;
/*        knockbackDirection = new Vector2((Mathf.Sign(knockbackDirection.x)), 0);
        Debug.Log(knockbackDirection);*/
        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);

        Debug.Log(rb.velocity);

        StartCoroutine(KnockbackDelay());
        StunEnemy();
    }

    protected IEnumerator KnockbackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector2.zero;

    }

    public void StunEnemy()
    {
        bossEnemyState = BossEnemyStates.STUNNED;
    }


    public override void OnDeath()
    {
        bossEnemyState = BossEnemyStates.DEATH;
        

        foreach (GameObject wall in wallsToSpawn)
        {
            wall.SetActive(false);
        }

        if (itemToSpawn != null && enemyParent.gameObject.activeSelf == true)
        {
            GameObject temp = Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
        enemyParent.gameObject.SetActive(false);

    }

    public BossEnemyStates GetBossEnemyState()
    {
        return bossEnemyState;
    }
}
