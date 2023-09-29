using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrollingChaserEnemyScript : EnemyScript
{
    

    // Update is called once per frame
    new private void Update()
    {
        //Debug.Log("New Update");
        //Check if the player collides with any of the circle colliders made in this code
        CheckCustomColliders();


        switch (enemyState)
        {
            case EnemyStates.IDLE:
                //If the enemy is meant to patrol, set the state to patrol

                if (currentlyAttacking == false)
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
                /*
                if (isChargingAttack == false)
                {
                    
                }
                */
                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                //if()
                ChasePlayer();
                /*
                if (isChargingAttack == false)
                {
                    
                }
                */
                if (canChaseDetection == false && enemyState == EnemyStates.CHASE && currentlyAttacking == false)
                {
                    ChangeToPatrol();
                }
                break;

            case EnemyStates.ATTACK:
                //Executes attack and changes attack state to counterable
                if (canChaseDetection == true && canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.CHASE;
                }



                if (canAttack == true)
                {
                    //StopAllCoroutines();

                    //Maybe put all of these functions into one coroutine because with the method I have now, all the coroutines activate at the same time

                    
                    StartCoroutine(EnemyAttack());


                }

                if(canAttackDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false || canChaseDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false)
                {
                    ChangeToPatrol();
                }

                break;

            case EnemyStates.STUNNED:
                //Enemy cannot move
                rb.velocity = new Vector2(0f, 0f);
                StartCoroutine(StunTimer());
                break;

            case EnemyStates.DEATH:
                //Enemy is dead and cannot do anything
                break;

            default:
                break;

        }
    }

    new protected void Movement()
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

        //If the enemy spots the player, switch to chase
        if (canChaseDetection == true)
        {
            enemyState = EnemyStates.CHASE;
        }

        /*
        if (isChargingAttack == false)
        {
            
        }
        */
        
    }

    new protected void CheckCustomColliders()
    {
       
        canChaseDetection = Physics2D.OverlapCircle(transform.position, chaseTriggerRadius, playerLayer);
        

        canAttackDetection = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);

        //Checks if the collider is not touching the ground
        //if one of the raycast is not touching the ground, the boolean is set to true
        onEdgeOfGround = !Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
            !Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //If the enemy is out of range of the chasing and attacking colliders while it is chasing or attack, set the state to patrol and turn off one of the stop points

        //Seperate each condition of the if statement and put them into the desinated state. Put the contents inside of the if statement in a seperate function

        /*
        if ((canChaseDetection == false && enemyState == EnemyStates.CHASE) ||
        (canChaseDetection == false && enemyState == EnemyStates.ATTACK) ||
        (canAttackDetection == false && enemyState == EnemyStates.ATTACK && currentlyAttacking == false) ||
        (canChaseDetection == false && enemyState == EnemyStates.CHASE && currentlyAttacking == false) || canAttackDetection == false && enemyState == EnemyStates.ATTACK && isChargingAttack == false)
        {


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

    new protected void ChasePlayer()
    {
        //Changes the direction the enemy based on which side the player is on
        if (transform.position.x < playerTarget.position.x)
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

        if (canAttackDetection == true)
        {
            enemyState = EnemyStates.ATTACK;
        }
        /*
        if (isChargingAttack == true)
        {
            enemyState = EnemyStates.ATTACK;
        }

        if (isChargingAttack == false)
        {

        }
        */
    }

    new protected void OnDrawGizmosSelected()
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


    new protected IEnumerator StunTimer()
    {
        mainSprite.color = Color.gray;
        yield return new WaitForSeconds(stunTime);

        mainSprite.color = mainColor;
        currentlyAttacking = false;
        isUsingAttack = false;
        canAttack = true;
        enemyState = EnemyStates.PATROL;
        

    }

    new protected void ChangeToPatrol()
    {
        Debug.Log("Change to Patrol");
        isChargingAttack = false;
        isUsingAttack = false;
        isRecharging = false;
        mainSprite.color = mainColor;
        attackState = AttackStates.NON_COUNTERABLE;

        enemyState = EnemyStates.PATROL;

        TurnOffOneStopPoint();


        //Rotates the enemy
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);

    }

}
