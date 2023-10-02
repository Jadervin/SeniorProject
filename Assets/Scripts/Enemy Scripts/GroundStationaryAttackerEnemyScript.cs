using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GroundStationaryAttackerEnemyScript : EnemyScript
{
    //[Header("Attack State Variables")]
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private SpriteRenderer attackSprite;


    new protected void Start()
    {
        enemyState = EnemyStates.IDLE;
        attackState = AttackStates.NON_COUNTERABLE;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        oldChaseTriggerRadius = chaseTriggerRadius;
        oldAttackTriggerRadius = attackTriggerRadius;

        counterableTimeFrame = attackTime;

        mainColor = mainSprite.color;

        attackCollider.enabled = false;
        attackSprite.enabled = false;

        

    }


    // Update is called once per frame
    new private void Update()
    {
        //Check if the player collides with any of the circle colliders made in this code
        CheckCustomColliders();
        isFacingRightFunction();

        switch (enemyState)
        {
            case EnemyStates.IDLE:
                //If the enemy is meant to patrol, set the state to patrol

                /*
                enemyState = EnemyStates.PATROL;


                else
                {
                    enemyState = EnemyStates.IDLE;
                }

                If the enemy spots the player, switch to chase
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                */
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                break;

            case EnemyStates.PATROL:
                //Movement
                /*
                Movement();

                //If the enemy spots the player, switch to chase
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                */
                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                /*
                ChasePlayer();
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                */
                break;

            case EnemyStates.ATTACK:
                //Executes attack and changes attack state to counterable
                /*
                if (canChaseDetection == true && canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.CHASE;
                }
                */

                TurnEnemy();

                if (canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.IDLE;
                }

                if (canAttack == true)
                {
                    //StopAllCoroutines();

                    //Maybe put all of these functions into one coroutine because with the method I have now, all the coroutines activate at the same time
                    StartCoroutine(EnemyAttack());


                }

                if (canAttackDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false)
                {
                    ChangeToPatrol();
                }


                break;

            case EnemyStates.STUNNED:
                //Enemy cannot move
                //rb.velocity = new Vector2(0f, 0f);
                if (stunTimerOn == false)
                {
                    StartCoroutine(StunTimer());
                    
                }
/*

                if (notStunnedAnymore == true)
                {
                    StunToIdle();
                }
*/
                break;

            case EnemyStates.DEATH:
                //Enemy is dead and cannot do anything
                break;

            default:
                break;
        }
    }


    new protected void CheckCustomColliders()
    {

        canAttackDetection = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);

        //Checks if the collider is not touching the ground
        //if one of the raycast is not touching the ground, the boolean is set to true
        onEdgeOfGround = !Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
            !Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //If the enemy is out of range of the chasing and attacking colliders while it is chasing or attack, set the state to patrol and turn off one of the stop points
        /*
        if (canAttackDetection == false && enemyState == EnemyStates.ATTACK)
        {

            isChargingAttack = false;
            isUsingAttack = false;
            isRecharging = false;

            
            enemyState = EnemyStates.IDLE;

            //Rotates the enemy
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
            
        }
        */


        //if the enemy is on the edge of the ground:
        //set the chase to false
        //turn off one of the stop points
        //make the radiuses of the colliders zero
        //change the enemyState to patrol
        if (onEdgeOfGround && enemyState != EnemyStates.IDLE)
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

    new protected void OnDrawGizmosSelected()
    {
       
        /* 
            //Drawing the Chase Trigger Circle
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseTriggerRadius);
        */

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

    new protected IEnumerator EnemyAttack()
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

        //Enemy is now attacking
        isUsingAttack = true;

        if (isChargingAttack == false && isRecharging == false && isUsingAttack == true)
        {
            attackState = AttackStates.COUNTERABLE;

            
            /*
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;

            //Dash
            rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
            */
            attackCollider.enabled = true;
            attackSprite.enabled = true;

            yield return new WaitForSeconds(counterableTimeFrame);
            attackState = AttackStates.NON_COUNTERABLE;

            attackCollider.enabled = false;
            attackSprite.enabled = false;
            //rb.gravityScale = originalGravity;
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


    new protected IEnumerator StunTimer()
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


        enemyState = EnemyStates.IDLE;

        stunTimerOn = false;

    }
/*

    public void StunToIdle()
    {
        enemyState = EnemyStates.IDLE;
        notStunnedAnymore = false;
        //stunTimerOn = false;
    }
*/


    new protected void ChangeToPatrol()
    {
        Debug.Log("Change to Patrol");
        isChargingAttack = false;
        isUsingAttack = false;
        isRecharging = false;
        mainSprite.color = mainColor;
        attackState = AttackStates.NON_COUNTERABLE;

        enemyState = EnemyStates.IDLE;

        TurnOffOneStopPoint();


        //Rotates the enemy
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);

    }

    
}
