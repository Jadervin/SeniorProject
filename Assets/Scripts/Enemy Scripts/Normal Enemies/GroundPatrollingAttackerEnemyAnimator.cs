using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrollingAttackerEnemyAnimator : MonoBehaviour
{
    private Animator patrollingEnemyAnimator;

    private const string IS_CHARGING = "isCharging";
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_RECHARGING = "isRecharging";
    private const string IS_DONE_RECHARGING = "doneRecharging";



    // Start is called before the first frame update
    void Start()
    {
        patrollingEnemyAnimator = GetComponent<Animator>();

        GroundPatrollingAttackerEnemyScript.OnAnyEnemyCharging += GroundPatrollingAttackerEnemyScript_OnAnyEnemyCharging;
        GroundPatrollingAttackerEnemyScript.OnAnyEnemyDash += GroundPatrollingAttackerEnemyScript_OnAnyEnemyDash;
        GroundPatrollingAttackerEnemyScript.OnAnyEnemyRecharging += GroundPatrollingAttackerEnemyScript_OnAnyEnemyRecharging;
        GroundPatrollingAttackerEnemyScript.OnAnyEnemyDoneRecharging += GroundPatrollingAttackerEnemyScript_OnAnyEnemyDoneRecharging;
    }

    

    private void GroundPatrollingAttackerEnemyScript_OnAnyEnemyCharging(object sender, System.EventArgs e)
    {
        patrollingEnemyAnimator.SetTrigger(IS_CHARGING);
    }

    private void GroundPatrollingAttackerEnemyScript_OnAnyEnemyDash(object sender, System.EventArgs e)
    {
        patrollingEnemyAnimator.SetTrigger(IS_ATTACKING);
    }

    private void GroundPatrollingAttackerEnemyScript_OnAnyEnemyRecharging(object sender, System.EventArgs e)
    {
        patrollingEnemyAnimator.SetTrigger(IS_RECHARGING);
    }

    private void GroundPatrollingAttackerEnemyScript_OnAnyEnemyDoneRecharging(object sender, System.EventArgs e)
    {
        patrollingEnemyAnimator.SetTrigger(IS_DONE_RECHARGING);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
