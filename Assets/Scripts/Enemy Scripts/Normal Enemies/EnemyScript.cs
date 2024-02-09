using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    STUNNED,
    DEATH
}

public enum AttackCounterStates
{
    NON_COUNTERABLE,
    COUNTERABLE
}


public class EnemyScript : EntityScript
{

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D boxCollider;
    [SerializeField] protected Vector3 startPosition;

    [Header("Components")]
    [SerializeField] protected GameObject enemyParent;
    [SerializeField] protected GameObject leftStopPoint;
    [SerializeField] protected GameObject rightStopPoint;
    [SerializeField] protected Transform playerTarget;
    [SerializeField] protected SpriteRenderer mainSprite;
    [SerializeField] protected GameObject itemToSpawn;
    [SerializeField] protected string enemyID = "";
    protected Color mainColor;

    /*[Header("Damage Amount")]
    [SerializeField] protected int damage;*/

    [Header("Collider Settings")]
    //The length of the ground-checking collider
    [SerializeField] protected float groundLength = 0.95f;

    //The distance between the ground-checking colliders
    [SerializeField] protected Vector3 colliderOffset;

    [SerializeField] protected float chaseTriggerRadius = 3f;
    [SerializeField] protected float attackTriggerRadius = 1.5f;
    [SerializeField] protected float oldChaseTriggerRadius;
    [SerializeField] protected float oldAttackTriggerRadius;


    [Header("Movement Stats")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float chaseSpeedMultiplier = 1.3f;

    [Header("Booleans")]
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool onEdgeOfGround;
    [SerializeField] protected bool canChaseDetection;
    [SerializeField] protected bool canAttackDetection;
    [SerializeField] protected bool currentlyAttacking;
    

    [SerializeField] protected bool stunTimerOn;
    [SerializeField] protected bool notStunnedAnymore;

    [Header("Options")]
    [SerializeField] private bool canPatrolOption;
    [SerializeField] private bool canChaseOption = true;

    [Header("States")]
    [SerializeField] protected EnemyStates _enemyState;
    [SerializeField] protected EnemyStates enemyState
    {
        get
        {
            return _enemyState;
        }
        set
        {
            _enemyState = value;
            //Debug.Log(gameObject.name + " changed state to " + value.ToString());
        }
    }
    [SerializeField] protected AttackCounterStates _attackState;
    [SerializeField] protected AttackCounterStates attackState
    {
        get
        {
            return _attackState;
        }
        set
        {
            _attackState = value;
            //Debug.Log(gameObject.name + " changed state to " + value.ToString());
        }
    }
/*
    [Header("Tags")]
    public string STOP_POINT_TAG = "StopPoint";
    public string CAMERA_SWITCH_TRIGGER_TAG = "CameraSwitchTriggers";
    public string WALLTAG = "Wall";
    public string TONGUE_COUNTER_TAG = "TongueCounter";*/

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

    //[Header("Events")]
    public static event EventHandler<OnEnemyDefeatedEventArgs> OnAnyEnemyDefeated;
    

    public class OnEnemyDefeatedEventArgs : EventArgs
    {
        public GameObject enemyParent;
        public GameObject itemToSpawn;
    }

    public static event EventHandler OnAnyEnemyDash;

    public static void ResetStaticData()
    {
        OnAnyEnemyDash = null;
        OnAnyEnemyDefeated = null;
    }

    /*
    public event EventHandler<OnKnockbackEventArgs> OnEnemyKnockbackAction;
    public class OnKnockbackEventArgs : EventArgs
    {
        public GameObject collidedGameObject;
    }
    */

    // Start is called before the first frame update
    protected void Start()
    {
        playerTarget = FindAnyObjectByType<PlayerMovement>().transform;
        enemyState = EnemyStates.IDLE;
        attackState = AttackCounterStates.NON_COUNTERABLE;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<Collider2D>();

        oldChaseTriggerRadius = chaseTriggerRadius;
        oldAttackTriggerRadius = attackTriggerRadius;

        counterableTimeFrame = attackTime;

        mainColor = mainSprite.color;

        startPosition = this.transform.localPosition;

        //isFacingRightFunction();

    }

    public void ResetEnemyState()
    {
        enemyState = EnemyStates.IDLE;
        attackState = AttackCounterStates.NON_COUNTERABLE;
        mainSprite.color = mainColor;
        canAttack = true;
        isChargingAttack = false;
        isUsingAttack = false;
        isRecharging = false;
        this.transform.localPosition = startPosition;
    }

    // Update is called once per frame
    protected void Update()
    {
        //Debug.Log(rb.velocity);
        //Debug.Log("Update");
        //Check if the player collides with any of the circle colliders made in this code
        CheckCustomColliders();


        switch (enemyState)
        {
            case EnemyStates.IDLE:
                //If the enemy is meant to patrol, set the state to patrol
                if (canPatrolOption == true)
                {
                    enemyState = EnemyStates.PATROL;
                }

                else
                {
                    enemyState = EnemyStates.IDLE;
                }

                //If the enemy spots the player, switch to chase
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }

                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }

                break;

            case EnemyStates.PATROL:
                //Movement
                Movement();

                //If the enemy spots the player, switch to chase
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                //if()
                ChasePlayer();
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                if (canChaseDetection == false && enemyState == EnemyStates.CHASE && currentlyAttacking == false)
                {
                    ChangeToPatrol();
                }
                break;

            case EnemyStates.ATTACK:

                if (isChargingAttack == false || currentlyAttacking == false)
                {
                    TurnEnemy();
                }

                if (canChaseDetection == true && canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.CHASE;
                }

                if (canAttackDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false || canChaseDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false)
                {
                    ChangeToPatrol();
                }

                if (canAttack == true)
                {
                    //StopAllCoroutines();

                    //Maybe put all of these functions into one coroutine because with the method I have now, all the coroutines activate at the same time
                    StartCoroutine(EnemyAttack());


                }
                
                break;

            case EnemyStates.STUNNED:
                //Enemy cannot move

                if (stunTimerOn == false)
                {
                    StartCoroutine(StunTimer());
                    

                }

                chaseTriggerRadius = 0;
                attackTriggerRadius = 0;
                /*
                                if(notStunnedAnymore == true)
                                {
                                    StunToPatrolORIdle();
                                }
                */
                //rb.velocity = new Vector2(0f, 0f);
                break;

            case EnemyStates.DEATH: 
                //Enemy is dead and cannot do anything
                break;

            default: 
                break;
                    
        }
    }

    protected void Movement()
    {
        if (isChargingAttack == false)
        {
            //Moves in the direction the enemy is facing
            if (isFacingRightFunction())
            {
                rb.velocity = new Vector2(moveSpeed, 0f);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, 0f);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //if it collides with one of the stop points while patrolling, rotate the enemy
        if(collision.gameObject.CompareTag(TagReferencesScript.STOP_POINT_TAG) && enemyState == EnemyStates.PATROL)
        {
            //if one of the colliders were turned off because it exited the chase state or reached the edge of the ground, turn that collider on after it collides with the other one.
            if (collision.gameObject == rightStopPoint)
            {
                leftStopPoint.SetActive(true);
                //chaseTriggerRadius = oldChaseTriggerRadius;
                //attackTriggerRadius = oldAttackTriggerRadius;

            }
            else if(collision.gameObject == leftStopPoint)
            {
                rightStopPoint.SetActive(true);
                //chaseTriggerRadius = oldChaseTriggerRadius;
                //attackTriggerRadius = oldAttackTriggerRadius;
            }

            chaseTriggerRadius = oldChaseTriggerRadius;
            attackTriggerRadius = oldAttackTriggerRadius;

            //Rotates the enemy when it reaches a stop point
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            
        }

        if (collision.gameObject.CompareTag(TagReferencesScript.TONGUE_COUNTER_TAG) && enemyState == EnemyStates.ATTACK && attackState == AttackCounterStates.COUNTERABLE)
        {
            /*
            //Knockback enemy
            //OnEnemyKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
            //{
            //    collidedGameObject = collision.gameObject
            //});
            */
            //StopCoroutine(EnemyAttack());
            
            EnemyKnockbackAction(collision.gameObject);

            //enemyState = EnemyStates.STUNNED;
        }

    }

    private void OnParticleCollision(GameObject particle)
    {
        if (particle.GetComponent<SpecialWeaponParticleScript>().GetHitWithParticleBool() == false)
        {
            DamageHealth(particle.GetComponent<SpecialWeaponParticleScript>().GetDamage());
            particle.GetComponent<SpecialWeaponParticleScript>().SetHitWithParticleBool(true);

            
            //particle.SetActive(false);
            //hitWithParticle = true;
        }

    }


    protected void EnemyKnockbackAction(GameObject collidedGameObject)
    {
        //StopAllCoroutines();

        //Debug.Log(transform.position);
        //Debug.Log(collidedGameObject.transform.position);

        //Debug.Log(transform.position - collidedGameObject.transform.position);

        //StopAllCoroutines();
        Vector2 knockbackDirection = (transform.position - collidedGameObject.transform.position).normalized;
        knockbackDirection.y *= -1;
        Debug.Log(knockbackDirection * knockbackStrength);

        rb.velocity = Vector2.zero;
        //knockbackDirection = new Vector2((Mathf.Sign(knockbackDirection.x)), 0);
        //Debug.Log(knockbackDirection);
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
        enemyState = EnemyStates.STUNNED;
    }


    protected IEnumerator StunTimer()
    {
        stunTimerOn = true;
        mainSprite.color = Color.gray;


        currentlyAttacking = false;
        isUsingAttack = false;
        isRecharging = false;
        isChargingAttack = false;

        canAttack = true;

        yield return new WaitForSeconds(stunTime);

        mainSprite.color = mainColor;

        stunTimerOn = false;
        notStunnedAnymore = true;

        if (canPatrolOption == true)
        {
            enemyState = EnemyStates.PATROL;
        }
        else
        {
            enemyState = EnemyStates.IDLE;
        }

    }
/*
    private void StunToPatrolORIdle()
    {
        if (canPatrolOption == true)
        {
            enemyState = EnemyStates.PATROL;
        }
        else
        {
            enemyState = EnemyStates.IDLE;
        }

        //stunTimerOn = false;
        notStunnedAnymore = false;
    }
*/



    /*
    private void OnCollisionEnter2D(Collision2D collision)
     {
        if (collision.gameObject.CompareTag(WALLTAG) && enemyState == EnemyStates.PATROL)
        {


            //Rotates the enemy when it reaches a stop point
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
    }
    */

    //Checks if the player is facing right
    protected bool isFacingRightFunction()
    {
        isFacingRight = transform.localScale.x > Mathf.Epsilon;
        return transform.localScale.x > Mathf.Epsilon;
    }

    //Checks all of the custom colliders 
    protected void CheckCustomColliders()
    {
        if (canChaseOption == true)
        {
            canChaseDetection = Physics2D.OverlapCircle(transform.position, chaseTriggerRadius, playerLayer);
        }

        canAttackDetection = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);
        
        //Checks if the collider is not touching the ground
        //if one of the raycast is not touching the ground, the boolean is set to true
        onEdgeOfGround = !Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || 
            !Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //If the enemy is out of range of the chasing and attacking colliders while it is chasing or attack, set the state to patrol and turn off one of the stop points
        /*
        if(canChaseDetection == false && enemyState == EnemyStates.CHASE || canChaseDetection == false && enemyState == EnemyStates.ATTACK || canAttackDetection == false && enemyState == EnemyStates.ATTACK) 
        {

            isChargingAttack = false;
            isUsingAttack = false;
            isRecharging = false;

            if (canPatrolOption == true)
            {
                enemyState = EnemyStates.PATROL;

                TurnOffOneStopPoint();


                
                 //Rotates the enemy when it reaches a stop point
                 
            //Rotates the enemy
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            }
            else
            {
                enemyState = EnemyStates.IDLE;

                //Rotates the enemy
                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            }
        }
        */


        //if the enemy is on the edge of the ground:
        //set the chase to false
        //turn off one of the stop points
        //make the radiuses of the colliders zero
        //change the enemyState to patrol
        if (onEdgeOfGround && enemyState != EnemyStates.IDLE && enemyState != EnemyStates.STUNNED)
        {
            canChaseDetection = false;
            TurnOffOneStopPoint();
            chaseTriggerRadius = 0;
            attackTriggerRadius = 0;


            /*
             * //Rotates the enemy when it reaches a stop point
             */
            //Rotates the enemy
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            enemyState = EnemyStates.PATROL;
        }
    }


    //Draws the colliders
    protected void OnDrawGizmosSelected()
    {
        //Drawing the Chase Trigger Circle
        if (canChaseOption == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseTriggerRadius);
        }

        //Drawing the Attack Trigger Circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackTriggerRadius);

        //if the player is on the edge of the ground, turn the color to yellow, else turn it to green
        if (onEdgeOfGround)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    //Chases the player
    protected void ChasePlayer()
    {
        //Changes the direction the enemy based on which side the player is on
        if(transform.position.x < playerTarget.position.x)
        {
            rb.velocity = new Vector2(moveSpeed * chaseSpeedMultiplier, 0f);
            transform.localScale = new Vector2(1, transform.localScale.y);
            isFacingRightFunction();
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed * chaseSpeedMultiplier, 0f);
            transform.localScale = new Vector2(-1, transform.localScale.y);
            isFacingRightFunction();
        }
    }

    protected void TurnOffOneStopPoint()
    {
        //If the enemy is facing left and it's position is less than that of the leftStopPoint's position, turn off the leftStopPoint
        if (transform.localScale.x == -1 && transform.position.x < leftStopPoint.transform.position.x)
        {
            leftStopPoint.SetActive(false);
        }
        //Else if the enemy is facing right and it's position is greater than that of the rightStopPoint's position, turn off the rightStopPoint
        else if (transform.localScale.x == 1 && transform.position.x > rightStopPoint.transform.position.x)
        {
            rightStopPoint.SetActive(false);
        }
        //Else if the enemy is facing right and it's position is less than that of the rightStopPoint's position, turn off the leftStopPoint
        else if (transform.localScale.x == 1 && transform.position.x < rightStopPoint.transform.position.x)
        {
            leftStopPoint.SetActive(false);
        }
    }

    //This function overrides the one inheited by the entity script and when this script calls OnDamage and currentHealth = 0, this version of the function is called for the enemy.
    public override void OnDeath()
    {
        enemyState = EnemyStates.DEATH;
        //enemyParent.gameObject.SetActive(false);

        if (itemToSpawn != null && enemyParent.gameObject.activeSelf == true)
        {
            GameObject temp = Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
        enemyParent.gameObject.SetActive(false);

        OnAnyEnemyDefeated?.Invoke(this, new OnEnemyDefeatedEventArgs
        {
            enemyParent = enemyParent,
            itemToSpawn = itemToSpawn,
        });

        //Destroy(gameObject);
    }

    /*public int GetDamage()
    {
        return damage;
    }*/

    protected IEnumerator EnemyAttack() 
    {
        //Have the enemy charge up their attack
        canAttack = false;
        isChargingAttack = true;
        currentlyAttacking = true;
        mainSprite.color = Color.red;
        if (isUsingAttack == false && isRecharging == false && isChargingAttack == true)
        {
            yield return new WaitForSeconds(attackChargeTime);
            isChargingAttack = false;
        }
        //Attack
        //StartCoroutine(EnemyAttack_DashAttack());

        //Enemy is now attacking with the dash attack
        isUsingAttack = true;

        if (isChargingAttack == false && isRecharging == false && isUsingAttack == true)
        {
            attackState = AttackCounterStates.COUNTERABLE;

            OnAnyEnemyDash?.Invoke(this, EventArgs.Empty);

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
            attackState = AttackCounterStates.NON_COUNTERABLE;
            rb.gravityScale = originalGravity;
            currentlyAttacking = false;
            isUsingAttack = false;
        }

        mainSprite.color = mainColor;

        //Enemy is now recharging
        isRecharging = true;
        //if (isChargingAttack == false && isAttacking == false && isRecharging == true)
        
        yield return new WaitForSeconds(attackRechargeTime);

        isRecharging = false;
        canAttack = true;


    }

    protected void TurnEnemy()
    {
        //Changes the direction the enemy based on which side the player is on
        if (isUsingAttack == false)
        {
            if (transform.position.x < playerTarget.position.x)
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
                isFacingRightFunction();
            }
            else
            {

                transform.localScale = new Vector2(-1, transform.localScale.y);
                isFacingRightFunction();
            }
        }
    }

   
    protected void ChangeToPatrol()
    {
        Debug.Log("Change to Patrol");
        isChargingAttack = false;
        isUsingAttack = false;
        isRecharging = false;
        mainSprite.color = mainColor;
        attackState = AttackCounterStates.NON_COUNTERABLE;

        if (canPatrolOption == true)
        {
            enemyState = EnemyStates.PATROL;

            TurnOffOneStopPoint();


            /*
                Rotates the enemy when it reaches a stop point
             */

            //Rotates the enemy
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
        else
        {
            enemyState = EnemyStates.IDLE;

            //Rotates the enemy
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }

    }

    public string GetEnemyID()
    {
        return enemyID;
    }   
    
    

}

