using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStationaryChaserEnemyAnimator : MonoBehaviour
{

    private Animator flyingEnemyAnimator;

    [SerializeField] private FlyingStationaryChaserEnemyScript flyingEnemy;

    private const string IS_CHARGING = "isCharging";
    private const string IS_ATTACKING = "isAttacking";
    private const string IS_RESET = "isReset";


    //[SerializeField] private FlyingStationaryChaserEnemyScript flyingEnemyScript;


    // Start is called before the first frame update
    void Start()
    {
        flyingEnemyAnimator = GetComponent<Animator>();

        //flyingEnemyScript.
        flyingEnemy.OnEnemyChargingAnim += FlyingStationaryChaserEnemyScript_OnAnyEnemyCharging;
        flyingEnemy.OnEnemySlashAnim += FlyingStationaryChaserEnemyScript_OnAnyEnemySlash;
        flyingEnemy.OnEnemyResetAnim += FlyingStationaryChaserEnemyScript_OnAnyEnemyReset;
    }

    private void FlyingStationaryChaserEnemyScript_OnAnyEnemyReset(object sender, System.EventArgs e)
    {
        flyingEnemyAnimator.SetTrigger(IS_RESET);
    }

    private void FlyingStationaryChaserEnemyScript_OnAnyEnemyCharging(object sender, System.EventArgs e)
    {
        flyingEnemyAnimator.SetTrigger(IS_CHARGING);
    }

    private void FlyingStationaryChaserEnemyScript_OnAnyEnemySlash(object sender, System.EventArgs e)
    {
        flyingEnemyAnimator.SetTrigger(IS_ATTACKING);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
