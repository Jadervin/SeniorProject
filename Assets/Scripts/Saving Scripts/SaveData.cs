using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public Vector3 playerLocation;
    public int currentHealth;
    public int maxHealth;
    public int currentSWEnergy;
    public int maxSWEnergy;
    public BasicShootingUpgradeStates currentBaseBulletState;
    public int currentArtifactNum;
    public int maxArtifactNum;
    public int currentCameraIndex;

    public string[] enemyIDs;
    public string[] bossEnemyIDs;
    public string[] collectablesIDs;
}
