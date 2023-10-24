using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossEnemyStates
{
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

    [Header("Tags")]
    public string WALLTAG = "Wall";
    public string TONGUE_COUNTER_TAG = "TongueCounter";

    [Header("Layer Masks")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;


    [Header("Attack State Variables")]
    [SerializeField] protected float attackChargeTime = 5f;
    [SerializeField] protected float attackRechargeTime = 1f;

    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected float dashPower = 24.0f;
    [SerializeField] protected float attackTime = .4f;
    [SerializeField] protected float counterableTimeFrame = .3f;

    [SerializeField] protected bool isChargingAttack = false;
    [SerializeField] protected bool isUsingAttack = false;
    [SerializeField] protected bool isRecharging = false;

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


    }


    private void Update()
    {
        switch (bossEnemyState)
        {
            case BossEnemyStates.IDLE:
                
                break;

            case BossEnemyStates.WALKING:
                
                break;

            case BossEnemyStates.ATTACKING:
                
                break;

            case BossEnemyStates.STUNNED:
                
                break;

            case BossEnemyStates.DEATH:
                
                break;

            default:
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TONGUE_COUNTER_TAG) && bossEnemyState == BossEnemyStates.ATTACKING && attackCounterState == AttackCounterStates.COUNTERABLE)
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
        //enemyParent.gameObject.SetActive(false);

        if (itemToSpawn != null && enemyParent.gameObject.activeSelf == true)
        {
            GameObject temp = Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
        enemyParent.gameObject.SetActive(false);

    }
}
