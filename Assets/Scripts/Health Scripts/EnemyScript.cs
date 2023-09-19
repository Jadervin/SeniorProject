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

    [SerializeField] private EnemyStates enemyState;
    [SerializeField] private AttackStates attackState;
    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected bool isFacingRight;
    public string STOP_POINT_TAG = "StopPoint";

    [SerializeField] private float chaseTriggerRadius = 3f;
    [SerializeField] protected bool canChase;
    [SerializeField] private float attackTriggerRadius = 1.5f;
    [SerializeField] protected bool canAttack;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyStates.IDLE;
        attackState = AttackStates.NON_COUNTERABLE;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player collides with any of the circle colliders made in this code
        CheckCircleColliders();


        switch (enemyState)
        {
            case EnemyStates.IDLE:
                //Do nothing, just change to patrol state
                //Maybe when the player gets into a certain distance, change to patrol
                enemyState = EnemyStates.PATROL;
                break;

            case EnemyStates.PATROL:
                //Movement
                Movement();
                if (canChase == true)
                {
                    enemyState = EnemyStates.CHASE;
                }
                break;

            case EnemyStates.CHASE:
                //Track down and get into range of player
                if(canAttack == true)
                {
                    enemyState = EnemyStates.ATTACK;
                }
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

    protected void Movement()
    {
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
        if(collision.gameObject.CompareTag(STOP_POINT_TAG) && enemyState == EnemyStates.PATROL)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        }
        
    }

    protected bool isFacingRightFunction()
    {
        isFacingRight = transform.localScale.x > Mathf.Epsilon;
        return transform.localScale.x > Mathf.Epsilon;
    }

    protected void CheckCircleColliders()
    {
        canChase = Physics2D.OverlapCircle(transform.position, chaseTriggerRadius, playerLayer);

        canAttack = Physics2D.OverlapCircle(transform.position, attackTriggerRadius, playerLayer);

        if(canChase == false && enemyState == EnemyStates.CHASE || canChase == false && enemyState == EnemyStates.ATTACK) 
        {
            enemyState = EnemyStates.PATROL;

        }
    }


    protected void OnDrawGizmosSelected()
    {
        //Drawing the Chase Trigger Circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseTriggerRadius);


        //Drawing the Attack Trigger Circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackTriggerRadius);
    }


    public override void OnDeath()
    {
        enemyState = EnemyStates.DEATH;
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}

