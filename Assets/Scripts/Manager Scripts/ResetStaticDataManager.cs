using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        EnemyScript.ResetStaticData();
        BossEnemyScript.ResetStaticData();
        GroundStationaryAttackerEnemyScript.ResetStaticSoundEventData();
        FlyingStationaryChaserEnemyScript.ResetStaticSoundEventData();
        GroundPatrollingAttackerEnemyScript.ResetOtherStaticData();
        Collectables.ResetStaticData();
        RefillStationScript.ResetStaticData();
        PlanetTotemScript.ResetStaticData();
        
    }
}
