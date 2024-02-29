using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyAnimator : MonoBehaviour
{
    private Animator bossEnemyAnimator;

    private const string IS_ACTIVATED = "isActivated";
    private const string IS_SHOOTING = "isShooting";
    private const string IS_CHARGING = "isCharging";
    private const string IS_DASHING = "isDashing";
    private const string IS_RECHARGING = "isRecharging";
    private const string IS_STUNNED = "isStunned";
    //private const string IS_DONE_RECHARGING = "doneRecharging";



    // Start is called before the first frame update
    void Start()
    {
        bossEnemyAnimator = GetComponent<Animator>();
        BossEnemyScript.OnAnyBossActivation += BossEnemyScript_OnAnyBossActivation;
        BossEnemyScript.OnAnyBossShoot += BossEnemyScript_OnAnyBossShoot;
        BossEnemyScript.OnAnyBossEnemyCharging += BossEnemyScript_OnAnyBossEnemyCharging;
        BossEnemyScript.OnAnyBossDash += BossEnemyScript_OnAnyBossDash;
        BossEnemyScript.OnAnyBossEnemyRecharging += BossEnemyScript_OnAnyBossEnemyRecharging;
        BossEnemyScript.OnAnyBossStunned += BossEnemyScript_OnAnyBossStunned;
    }

    private void BossEnemyScript_OnAnyBossStunned(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_STUNNED);
    }

    private void BossEnemyScript_OnAnyBossEnemyRecharging(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_RECHARGING);
    }

    private void BossEnemyScript_OnAnyBossDash(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_DASHING);
    }

    private void BossEnemyScript_OnAnyBossEnemyCharging(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_CHARGING);
    }

    private void BossEnemyScript_OnAnyBossShoot(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_SHOOTING);
    }

    private void BossEnemyScript_OnAnyBossActivation(object sender, System.EventArgs e)
    {
        bossEnemyAnimator.SetTrigger(IS_ACTIVATED);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
