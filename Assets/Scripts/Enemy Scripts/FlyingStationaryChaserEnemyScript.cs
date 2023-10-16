using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStationaryChaserEnemyScript : EnemyScript
{
    [Header("AI Variables")]
    [SerializeField] private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    [SerializeField] private float repeatRate = .5f;



    new protected void Start()
    {
        playerTarget = FindAnyObjectByType<PlayerMovement>().transform;
        enemyState = EnemyStates.IDLE;
        attackState = AttackStates.NON_COUNTERABLE;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        oldChaseTriggerRadius = chaseTriggerRadius;
        oldAttackTriggerRadius = attackTriggerRadius;

        counterableTimeFrame = attackTime;

        mainColor = mainSprite.color;

        //isFacingRightFunction();

        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, repeatRate);

    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, playerTarget.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    new protected void Update()
    {
        //Debug.Log(rb.velocity);
        //Debug.Log("Update");
        //Check if the player collides with any of the circle colliders made in this code
        CheckCustomColliders();
        if(path == null)
        {
            return;
        }

        switch (enemyState)
        {
            case EnemyStates.IDLE:
                
                enemyState = EnemyStates.IDLE;
                

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
                
                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                //if()
                //ChasePlayer();
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                if (canChaseDetection == false && enemyState == EnemyStates.CHASE && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.IDLE;
                }
                break;

            case EnemyStates.ATTACK:

                if (canChaseDetection == true && canAttackDetection == false && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.CHASE;
                }

                /*
                                if (isChargingAttack == false || currentlyAttacking == false)
                                {
                                    TurnEnemy();
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
                */
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

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyStates.IDLE:

                break;

            case EnemyStates.PATROL:

                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                //if()
                ChasePlayer();
                /*if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                if (canChaseDetection == false && enemyState == EnemyStates.CHASE && currentlyAttacking == false)
                {
                    enemyState = EnemyStates.IDLE;
                }*/
                break;

            case EnemyStates.ATTACK:

                break;

            case EnemyStates.STUNNED:
                
                break;

            default:
                break;

        }
    }


    new protected void CheckCustomColliders()
    {
        
        canChaseDetection = Physics2D.OverlapCircle(transform.position, chaseTriggerRadius, playerLayer);
        

        canAttackDetection = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);

        

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


    }



    new protected void ChasePlayer()
    {
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * (moveSpeed * chaseSpeedMultiplier) * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


        /*//Changes the direction the enemy based on which side the player is on
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
        }*/
    }
}
