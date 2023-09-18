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


public abstract class  EnemyScript : EntityScript
{

    [SerializeField] private EnemyStates enemyState;
    [SerializeField] private AttackStates attackState;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyStates.IDLE;
        attackState = AttackStates.NON_COUNTERABLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(enemyState)
        {
            case EnemyStates.IDLE:
                //Do nothing, unless I want the enemy to move as an idle state
                break;
            case EnemyStates.PATROL: 
                //Movement
                break;
            case EnemyStates.CHASE:
                //Track down and get into range of player
                break;
            case EnemyStates.ATTACK: 
                //Executes attack and changes attack state to counterable
                break;
            case EnemyStates.STUNNED: 
                //Enemy cannot move
                break;
            case EnemyStates.DEATH: 
                //Enemy is dead and cannot do anything
                break;
            default: 
                break;
                    
        }
    }

    public override void OnDeath()
    {
        this.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
