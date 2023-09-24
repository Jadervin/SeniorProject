using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingAttackerEnemyScript : EnemyScript
{//This enemy may be a slow mover, but can have a wide attack range

    // Update is called once per frame
    new private void Update()
    {
        //Check if the player collides with any of the circle colliders made in this code
        CheckCustomColliders();


        switch (enemyState)
        {
            case EnemyStates.IDLE:

                enemyState = EnemyStates.PATROL;

                
                /*
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                */
                /*
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
                */
                break;

            case EnemyStates.PATROL:
                //Movement
                Movement();

                //If the enemy spots the player, switch to chase
                /*
                if (canChaseDetection == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                */
                //If the enemy spots the player, switch to attack
                if (canAttackDetection == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
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
}
