using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundStationaryAttackerEnemyAnimator : MonoBehaviour
{
    private Animator stationaryEnemyAnimator;
    [SerializeField] private GroundStationaryAttackerEnemyScript stationaryEnemy;

    private const string IS_CHARGING = "isCharging";
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_RECHARGING = "isRecharging";
    private const string IS_DONE_RECHARGING = "doneRecharging";
    private const string IS_RESET = "isReset";


    // Start is called before the first frame update
    void Start()
    {
        stationaryEnemyAnimator = GetComponent<Animator>();


        stationaryEnemy.OnEnemyChargingAnim += GroundStationaryAttackerEnemyScript_OnAnyEnemyCharging;
        stationaryEnemy.OnEnemySlashAnim += GroundStationaryAttackerEnemyScript_OnAnyEnemySlash;
        stationaryEnemy.OnEnemyRechargingAnim += GroundStationaryAttackerEnemyScript_OnAnyEnemyRecharging;
        stationaryEnemy.OnEnemyDoneRechargingAnim += GroundStationaryAttackerEnemyScript_OnAnyEnemyDoneRecharging;
        stationaryEnemy.OnEnemyResetAnim += GroundStationaryAttackerEnemyScript_OnAnyEnemyReset;

    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemyReset(object sender, System.EventArgs e)
    {
        stationaryEnemyAnimator.SetTrigger(IS_RESET);
    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemyCharging(object sender, System.EventArgs e)
    {
        stationaryEnemyAnimator.SetTrigger(IS_CHARGING);
    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemySlash(object sender, System.EventArgs e)
    {
        stationaryEnemyAnimator.SetTrigger(IS_ATTACKING);
    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemyRecharging(object sender, System.EventArgs e)
    {
        stationaryEnemyAnimator.SetTrigger(IS_RECHARGING);
    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemyDoneRecharging(object sender, System.EventArgs e)
    {
        stationaryEnemyAnimator.SetTrigger(IS_DONE_RECHARGING);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
