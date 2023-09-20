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

public enum AttackStates
{
    NON_COUNTERABLE,
    COUNTERABLE
}


public class EnemyScript : EntityScript
{
    
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;

    [Header("Components")]
    [SerializeField] protected GameObject enemyParent;
    [SerializeField] protected GameObject leftStopPoint;
    [SerializeField] protected GameObject rightStopPoint;
    [SerializeField] protected Transform playerTarget;

    [Header("Damage Amount")]
    [SerializeField] protected int damage;

    [Header("Collider Settings")]
    //The length of the ground-checking collider
    [SerializeField] protected float groundLength = 0.95f;

    //The distance between the ground-checking colliders
    [SerializeField] protected Vector3 colliderOffset;

    [SerializeField] protected float chaseTriggerRadius = 3f;
    [SerializeField] protected float attackTriggerRadius = 1.5f;
    protected float oldChaseTriggerRadius;
    protected float oldAttackTriggerRadius;


    [Header("Movement Stats")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float chaseSpeedMultiplier = 1.3f;

    [Header("Booleans")]
    [SerializeField] protected bool isFacingRight;
    [SerializeField] protected bool onEdgeOfGround;
    [SerializeField] protected bool canChaseDetection;
    [SerializeField] protected bool canAttackDetection;
    [SerializeField] protected bool currentlyAttacking;

    [Header("Options")]
    [SerializeField] protected bool canPatrolOption;

    [Header("States")]
    [SerializeField] protected EnemyStates enemyState;
    [SerializeField] protected AttackStates attackState;

    [Header("Tags")]
    public string STOP_POINT_TAG = "StopPoint";
    public string CAMERA_SWITCH_TRIGGER_TAG = "CameraSwitchTriggers";
    public string WALLTAG = "Wall";

    [Header("Layer Masks")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;


    [Header("Attack State Variables Masks")]
    [SerializeField] protected float attackChargeTime = 5f;
    [SerializeField] protected float counterableTimeFrameMax = .3f;
    //[SerializeField] protected float currentCounterableTimeFrame;
    [SerializeField] protected float attackRechargeTimeMax = 1f;
    //[SerializeField] protected float currentRechargeTime;

    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected float dashPower = 24.0f;
    [SerializeField] protected float attackTime = .4f;


    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyStates.IDLE;
        attackState = AttackStates.NON_COUNTERABLE;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        oldChaseTriggerRadius = chaseTriggerRadius;
        oldAttackTriggerRadius = attackTriggerRadius;

        counterableTimeFrameMax = attackTime;

    }

    // Update is called once per frame
    void Update()
    {
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

                //If the enemy spots the player, switch to chase
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
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
                break;

            case EnemyStates.ATTACK: 
                //Executes attack and changes attack state to counterable
                if(canChaseDetection == true && canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.CHASE;
                }

                

                if (canAttack == true)
                {
                    //StopAllCoroutines();
                    //Have the enemy charge up their attack
                    StartCoroutine(EnemyAttackStartUp());
                    
                    StartCoroutine(EnemyAttack_DashAttack());

                    StartCoroutine(AttackRecharge());

                }
                //while (currentlyAttacking == false && currentRechargeTime < attackRechargeTimeMax)
                //{
                //    //EnemyAttack_DashAttack();
                //    currentRechargeTime += Time.deltaTime;
                //}
                //else
                //{
                //    currentRechargeTime = 0;
                //    canAttack = true;
                //}

                break;

            case EnemyStates.STUNNED:
                //Enemy cannot move
                rb.velocity = new Vector2(0f, 0f); 
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it collides with one of the stop points while patrolling, rotate the enemy
        if(collision.gameObject.CompareTag(STOP_POINT_TAG) && enemyState == EnemyStates.PATROL)
        {
            //if one of the colliders were turned off because it exited the chase state or reached the edge of the ground, turn that collider on after it collides with the other one.
            if (collision.gameObject == rightStopPoint)
            {
                leftStopPoint.SetActive(true);
                chaseTriggerRadius = oldChaseTriggerRadius;
                attackTriggerRadius = oldAttackTriggerRadius;

            }
            else if(collision.gameObject == leftStopPoint)
            {
                rightStopPoint.SetActive(true);
                chaseTriggerRadius = oldChaseTriggerRadius;
                attackTriggerRadius = oldAttackTriggerRadius;
            }
            
            //Rotates the enemy when it reaches a stop point
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            
        }

        

    }


    //private void OnCollisionEnter2D(Collision2D collision)
    // {
    //    if (collision.gameObject.CompareTag(WALLTAG) && enemyState == EnemyStates.PATROL)
    //    {


    //        //Rotates the enemy when it reaches a stop point
    //        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
    //    }
    //}


    //Checks if the player is facing right
    protected bool isFacingRightFunction()
    {
        isFacingRight = transform.localScale.x > Mathf.Epsilon;
        return transform.localScale.x > Mathf.Epsilon;
    }

    //Checks all of the custom colliders 
    protected void CheckCustomColliders()
    {
        canChaseDetection = Physics2D.OverlapCircle(transform.position, chaseTriggerRadius, playerLayer);

        canAttackDetection = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);
        
        //Checks if the collider is not touching the ground
        //if one of the raycast is not touching the ground, the boolean is set to true
        onEdgeOfGround = !Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || 
            !Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //If the enemy is out of range of the chasing and attacking colliders while it is chasing or attack, set the state to patrol and turn off one of the stop points
        if(canChaseDetection == false && enemyState == EnemyStates.CHASE || canChaseDetection == false && enemyState == EnemyStates.ATTACK) 
        {

            if (canPatrolOption == true)
            {
                enemyState = EnemyStates.PATROL;

                TurnOffOneStopPoint();


                //Rotates the enemy when it reaches a stop point
                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            }
            else
            {
                enemyState = EnemyStates.IDLE;

                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            }
        }

       

        //if the enemy is on the edge of the ground:
            //set the chase to false
            //turn off one of the stop points
            //make the radiuses of the colliders zero
            //change the enemyState to patrol
        if (onEdgeOfGround)
        {
            canChaseDetection = false;
            TurnOffOneStopPoint();
            chaseTriggerRadius = 0;
            attackTriggerRadius = 0;


            //Rotates the enemy when it reaches a stop point
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            enemyState = EnemyStates.PATROL;
        }
    }


    //Draws the colliders
    protected void OnDrawGizmosSelected()
    {
        //Drawing the Chase Trigger Circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseTriggerRadius);


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
        enemyParent.gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public int GetDamage()
    {
        return damage;
    }

    public IEnumerator EnemyAttackStartUp()
    {
        yield return new WaitForSeconds(attackChargeTime);

    }


    public IEnumerator EnemyAttack_DashAttack()
    {
        attackState = AttackStates.COUNTERABLE;
        canAttack = false;
        currentlyAttacking = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        //Dash
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);


        //while (currentCounterableTimeFrame < counterableTimeFrameMax)
        //{

            
        //    currentCounterableTimeFrame += Time.deltaTime;

        //}
        //else
        //{
        //    currentCounterableTimeFrame = 0;
            
        //}

        yield return new WaitForSeconds(counterableTimeFrameMax);
        attackState = AttackStates.NON_COUNTERABLE;
        rb.gravityScale = originalGravity;
        currentlyAttacking = false;

    }


    public IEnumerator AttackRecharge()
    {
        yield return new WaitForSeconds(attackRechargeTimeMax);
        canAttack = true;
    }

}

