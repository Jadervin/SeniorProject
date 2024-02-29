using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    DASH,

}

public enum BossEnemyHealthStates
{
    HIGHHEALTH,
    MILDHEALTH,
    LOWHEALTH
}


public class BossEnemyScript : EntityScript
{



    [Header("States")]
    /*[SerializeField] */protected BossEnemyStates bossEnemyState;
   
    /*[SerializeField] */protected BossEnemyAttackStates bossAttackState;
    /*[SerializeField] */protected BossEnemyAttackStates previousBossAttackState;

    /*[SerializeField] */protected AttackCounterStates attackCounterState;
    /*[SerializeField] */protected BossEnemyHealthStates enemyHealthState;
    /*[SerializeField] */protected int maxAttackStates;


    //[SerializeField] protected List<BossEnemyAttackStates> bossAttackStateChoices;
    //[SerializeField] protected bool choseAttack = false;


    [Header("Components")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D polyCollider;
    [SerializeField] protected GameObject enemyParent;
    [SerializeField] protected Transform playerTarget;
    [SerializeField] protected SpriteRenderer mainSprite;
    [SerializeField] protected GameObject itemToSpawn;

    [SerializeField] private GameObject bossEnemyExplosionPrefab;

    protected Color mainColor;
    protected Color currentColor;
    [SerializeField] protected string bossEnemyID = "";

    [Header("Booleans")]
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool currentlyAttacking;

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
    protected float playerTriggerRadiusReset;
    [SerializeField] protected List<GameObject> wallsToSpawn = new List<GameObject>();

    [Header("Idle State Variables")]
    [SerializeField] protected float idleWaitTime = 3f;
    [SerializeField] protected float timeIdle = 0f;

    [Header("Attack State Variables")]
    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected bool isUsingAttack = false;
    [SerializeField] protected bool isRecharging = false;
    [SerializeField] protected float rechargeTime = 3f;

    [Header("Stun Variables")]
    [SerializeField] protected float stunWaitTime = 2f;
    [SerializeField] protected float timeStunned;
    [SerializeField] protected bool stunTimerOn;
    [SerializeField] protected bool notStunnedAnymore = true;

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
    /*[SerializeField] */protected float counterableTimeFrame = .3f;

    [SerializeField] protected bool isChargingAttack = false;
    

    [Header("Knockback Variables")]
    [SerializeField] protected float knockbackStrength = 16;
    [SerializeField] protected float delayTime = .3f;


    public static event EventHandler<OnBossEnemyDefeatedEventArgs> OnAnyBossEnemyDefeated;

    public class OnBossEnemyDefeatedEventArgs : EventArgs
    {
        public GameObject enemyParent;
        public GameObject itemToSpawn;
    }

    public static event EventHandler OnAnyBossShoot;
    public static event EventHandler OnAnyBossDash;

    public static event EventHandler OnAnyBossActivation;

    public static event EventHandler OnAnyBossEnemyCharging;
    public static event EventHandler OnAnyBossEnemyRecharging;

    public static event EventHandler OnAnyBossStunned;

    //public static event EventHandler OnAnyBossEnemyDoneRecharging;


    public static void ResetStaticData()
    {
        OnAnyBossDash = null;
        OnAnyBossShoot = null;
        OnAnyBossEnemyDefeated = null;
        OnAnyBossActivation = null;

        OnAnyBossEnemyCharging = null;
        OnAnyBossEnemyRecharging = null;

        OnAnyBossStunned = null;

        //OnAnyBossEnemyDoneRecharging = null;
    }


    private void Start()
    {
        playerTarget = FindAnyObjectByType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();
        polyCollider = GetComponent<Collider2D>();

        canSeePlayer = false;
        counterableTimeFrame = attackTime;

        mainColor = mainSprite.color;

        bossEnemyState = BossEnemyStates.WAITINGFORPLAYER;
        playerTriggerRadiusReset = playerTriggerRadius;


        timeIdle = 0f;
        timeStunned = 0f;
        notStunnedAnymore = true;

        foreach (GameObject wall in wallsToSpawn)
        {
            wall.SetActive(false);
        }

        maxAttackStates = Enum.GetValues(typeof(BossEnemyAttackStates)).Cast<int>().Max();

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
                if (stunTimerOn == false)
                {
                    //StartCoroutine(IdleTimer());
                    TurnEnemy();

                    timeIdle += Time.deltaTime;

                    if (timeIdle > idleWaitTime)
                    {
                        timeIdle = 0f;
                        bossEnemyState = BossEnemyStates.ATTACKING;
                    }
                }
                break;

            case BossEnemyStates.WALKING:
                
                break;

            case BossEnemyStates.ATTACKING:


                if (previousBossAttackState == BossEnemyAttackStates.NONE)
                {
                    bossAttackState = BossEnemyAttackStates.SHOOT;
                }
                else if (previousBossAttackState == BossEnemyAttackStates.SHOOT)
                {
                    bossAttackState = BossEnemyAttackStates.DASH;
                }
                else
                {
                    bossAttackState = BossEnemyAttackStates.SHOOT;
                }

                /*if (choseAttack == false)
                {
                    bossAttackState = (BossEnemyAttackStates)UnityEngine.Random.Range(1, maxAttackStates);
                    choseAttack = true;
                }*/

                switch(bossAttackState)
                {
                    case BossEnemyAttackStates.SHOOT:

                        if (canAttack == true)
                        {
                            canAttack = false;
                            StartCoroutine(Shooting());
                        }

                        break;

                    case BossEnemyAttackStates.DASH:
                        if (canAttack == true)
                        {
                            canAttack = false;
                            StartCoroutine(Dashing());
                        }

                        break;

               
                    default:

                        break;
                }


                break;

            case BossEnemyStates.STUNNED:
                if(stunTimerOn == false)
                {
                    stunTimerOn = true;
                    notStunnedAnymore = false;

                    StartCoroutine(StunTimer());
                }
                break;

            case BossEnemyStates.DEATH:
                
                break;

            default:
                break;

        }


        switch(enemyHealthState) 
        {
            case BossEnemyHealthStates.HIGHHEALTH:
                if (bossEnemyState != BossEnemyStates.WAITINGFORPLAYER && canAttack == true && bossEnemyState != BossEnemyStates.STUNNED)
                {
                    mainSprite.color = mainColor;
                    currentColor = mainColor;
                }

                if (currentHealth < (maxHealth * .6f))
                {
                    enemyHealthState = BossEnemyHealthStates.MILDHEALTH;
                }
                break;

            case BossEnemyHealthStates.MILDHEALTH:
                if (canAttack == true && bossEnemyState != BossEnemyStates.STUNNED)
                {
                    mainSprite.color = Color.magenta;
                    currentColor = Color.magenta;
                }


                if (currentHealth < (maxHealth * .3f))
                {
                    enemyHealthState = BossEnemyHealthStates.LOWHEALTH;
                }
                break;

            case BossEnemyHealthStates.LOWHEALTH:
                if (canAttack == true && bossEnemyState != BossEnemyStates.STUNNED)
                {
                    mainSprite.color = Color.red;
                    currentColor = Color.red;
                }
                break;

            default:

                break;


        }
    }

    private void CheckForPlayer()
    {
        playerTriggerRadius = playerTriggerRadiusReset;

        canSeePlayer = Physics2D.OverlapCircle(transform.position, playerTriggerRadius, playerLayer);


        if (canSeePlayer == true)
        {
            playerTriggerRadius = 0;
            mainSprite.color = mainColor;

            foreach(GameObject wall in wallsToSpawn)
            {
                wall.SetActive(true);
            }

            OnAnyBossActivation?.Invoke(this, EventArgs.Empty);

            bossEnemyState = BossEnemyStates.IDLE;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        
        //Drawing the Attack Trigger Circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerTriggerRadius);

    }

    /*    private IEnumerator IdleTimer()
        {
            yield return new WaitForSeconds(idleWaitTime);
            //choseAttack = false;
            bossEnemyState = BossEnemyStates.ATTACKING;
            //canAttack = true;

        }*/

    protected void TurnEnemy()
    {
        //Changes the direction the enemy based on which side the player is on
        
        if (transform.position.x < playerTarget.position.x)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);

            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            shootPoint.transform.rotation = Quaternion.Euler(rotator);


            //shootPoint.transform.Rotate(transform.rotation.x, 0f, transform.rotation.z);
            //transform.Rotate(transform.rotation.x, 0f, transform.rotation.z);
            isFacingRightFunction();
        }
        else
        {

            transform.localScale = new Vector2(-1, transform.localScale.y);

            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            shootPoint.transform.rotation = Quaternion.Euler(rotator);


            //shootPoint.transform.Rotate(transform.rotation.x, 180.0f, transform.rotation.z);
            //transform.Rotate(transform.rotation.x, 180f, transform.rotation.z);
            isFacingRightFunction();
        }
        
    }

    protected bool isFacingRightFunction()
    {
        isFacingRight = transform.localScale.x > Mathf.Epsilon;
        return transform.localScale.x > Mathf.Epsilon;
    }

    private IEnumerator Shooting()
    {
        isUsingAttack = true;

        for(int i = 0; i < numOfShots; i++)
        {
            OnAnyBossShoot?.Invoke(this, EventArgs.Empty);
            GameObject temp = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);
            //Debug.Log("Spawned Bullet");
            yield return new WaitForSeconds(shootReloadTime);
        }

        bossEnemyState = BossEnemyStates.IDLE;
        previousBossAttackState = BossEnemyAttackStates.SHOOT;

        //yield return new WaitForSeconds(rechargeTime);
        isUsingAttack = false;
        canAttack = true;
    }


    private IEnumerator Dashing()
    {
        
        //canAttack = false;
        isChargingAttack = true;
        currentlyAttacking = true;
        mainSprite.color = Color.yellow;

        OnAnyBossEnemyCharging?.Invoke(this, EventArgs.Empty);

        if (isUsingAttack == false && isRecharging == false && isChargingAttack == true)
        {
            yield return new WaitForSeconds(dashAttackChargeTime);
            isChargingAttack = false;
        }

        //Enemy is now attacking with the dash attack
        isUsingAttack = true;

        if (isChargingAttack == false && isRecharging == false && isUsingAttack == true)
        {
            attackCounterState = AttackCounterStates.COUNTERABLE;
            OnAnyBossDash?.Invoke(this, EventArgs.Empty);

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;

            //Dash
            rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);

            /*
            while (currentCounterableTimeFrame < counterableTimeFrameMax)
            {


                currentCounterableTimeFrame += Time.deltaTime;

            }
            else
            {
                currentCounterableTimeFrame = 0;

            }
            */
            yield return new WaitForSeconds(counterableTimeFrame);
            attackCounterState = AttackCounterStates.NON_COUNTERABLE;
            rb.gravityScale = originalGravity;
            currentlyAttacking = false;
            isUsingAttack = false;
        }

        yield return new WaitForSeconds(dashAttackChargeTime);


        /*if (stunTimerOn == false && bossEnemyState != BossEnemyStates.STUNNED)
        {}*/
        mainSprite.color = currentColor;
        bossEnemyState = BossEnemyStates.IDLE;

        OnAnyBossEnemyRecharging?.Invoke(this, EventArgs.Empty);


        previousBossAttackState = BossEnemyAttackStates.DASH;
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.TONGUE_COUNTER_TAG) && bossEnemyState == BossEnemyStates.ATTACKING && attackCounterState == AttackCounterStates.COUNTERABLE)
        {
            //Activate Enemy Countered Animation
            OnAnyBossStunned?.Invoke(this, EventArgs.Empty);

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

        //Debug.Log(knockbackDirection * knockbackStrength);

        rb.velocity = Vector2.zero;
/*        knockbackDirection = new Vector2((Mathf.Sign(knockbackDirection.x)), 0);
        Debug.Log(knockbackDirection);*/
        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);

        //Debug.Log(rb.velocity);

        StartCoroutine(KnockbackDelay());

        bossEnemyState = BossEnemyStates.STUNNED;
        //StunEnemy();
    }

    protected IEnumerator KnockbackDelay()
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector2.zero;
        //bossEnemyState = BossEnemyStates.STUNNED;

    }

/*    public void StunEnemy()
    {
        bossEnemyState = BossEnemyStates.STUNNED;
    }*/

    protected IEnumerator StunTimer()
    {
        
        mainSprite.color = Color.gray;


        currentlyAttacking = false;
        isUsingAttack = false;
        isRecharging = false;
        isChargingAttack = false;

        //canAttack = true;

        yield return new WaitForSeconds(stunWaitTime);

        mainSprite.color = currentColor;

        stunTimerOn = false;
        notStunnedAnymore = true;


    }


    public override void OnDeath()
    {
        bossEnemyState = BossEnemyStates.DEATH;

        polyCollider.enabled = false;

        GameObject explosionEffect = Instantiate(bossEnemyExplosionPrefab, this.transform.position, this.transform.rotation);


        foreach (GameObject wall in wallsToSpawn)
        {
            wall.SetActive(false);
        }

        if (itemToSpawn != null && enemyParent.gameObject.activeSelf == true)
        {
            GameObject temp = Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
        enemyParent.gameObject.SetActive(false);


        OnAnyBossEnemyDefeated?.Invoke(this, new OnBossEnemyDefeatedEventArgs
        {
            enemyParent = enemyParent,
            itemToSpawn = itemToSpawn,
        });

    }

    public string GetBossEnemyID()
    {
        return bossEnemyID;
    }

    public BossEnemyStates GetBossEnemyState()
    {
        return bossEnemyState;
    }
}
