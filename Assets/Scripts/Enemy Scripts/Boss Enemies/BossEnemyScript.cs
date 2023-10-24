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
    SHOOT,
    DASH
}



public class BossEnemyScript : MonoBehaviour
{

    public BossEnemyStates bossEnemyState;
    public BossEnemyAttackStates bossAttackState;
    public AttackCounterStates attackCounterState;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
