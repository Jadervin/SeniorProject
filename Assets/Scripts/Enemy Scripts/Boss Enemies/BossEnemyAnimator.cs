using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyAnimator : MonoBehaviour
{
    private Animator bossEnemyAnimator;
    [SerializeField] private BossEnemyScript bossEnemy;

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
        bossEnemy.OnBossActivationAnim += BossEnemyScript_OnAnyBossActivation;
        bossEnemy.OnBossShootAnim += BossEnemyScript_OnAnyBossShoot;
        bossEnemy.OnBossEnemyChargingAnim += BossEnemyScript_OnAnyBossEnemyCharging;
        bossEnemy.OnBossDashAnim += BossEnemyScript_OnAnyBossDash;
        bossEnemy.OnBossEnemyRechargingAnim += BossEnemyScript_OnAnyBossEnemyRecharging;
        bossEnemy.OnBossStunnedAnim += BossEnemyScript_OnAnyBossStunned;
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
