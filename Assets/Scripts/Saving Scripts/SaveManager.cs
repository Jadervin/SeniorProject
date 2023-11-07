using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    [SerializeField] private GameObject playerObject;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        SaveSystem.Initialize();
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (SaveSystem.GetGameStartState() == SaveSystem.GameStartStates.LOADGAME)
        {
            Load();

        }
        SaveSystem.SetGameStartState(SaveSystem.GameStartStates.BLANK);

    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Save()
    {
        SaveData saveData = new SaveData
        {
            playerLocation = playerObject.transform.position,
            currentHealth = playerObject.GetComponent<PlayerHealth>().GetCurrentHealth(),
            maxHealth = playerObject.GetComponent<PlayerHealth>().GetMaxHealth(),
            currentSWEnergy = playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().GetCurrentWeaponEnergy(),
            maxSWEnergy = playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().GetMaxWeaponEnergy(),
            currentArtifactNum = playerObject.GetComponent<PlayerArtifactCollection>().GetCurrentArtifactsCollected(),
            maxArtifactNum = playerObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded(),
            currentBaseBulletState = playerObject.GetComponentInChildren<PlayerBasicShooting>().GetBaseShootingState(),
            currentCameraIndex = CameraManager.instance.GetCurrentCameraIndex(),
            enemyIDs = EnemyManager.instance.GetEnemyIDs(),
            bossEnemyIDs = EnemyManager.instance.GetBossEnemyIDs(),
            collectablesIDs = StrayCollectableManager.Instance.GetCollectableIDs(),
            openRoomNames = MapRoomManager.instance.GetOpenRoomNames()
        };

        string json = JsonUtility.ToJson(saveData);
        SaveSystem.Save(json);

        Debug.Log("Saved");
        Debug.Log(json);

    }
    private void Load()
    {
        string saveString = SaveSystem.Load();

        if (saveString != null)
        {
            //Debug.Log("Loaded");
            Debug.Log(saveString);


            SaveData saveData = JsonUtility.FromJson<SaveData>(saveString);

            playerObject.transform.position = saveData.playerLocation;
            playerObject.GetComponent<PlayerHealth>().SetCurrentHealth(saveData.currentHealth);
            playerObject.GetComponent<PlayerHealth>().SetMaxHealth(saveData.maxHealth);
            playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().SetCurrentWeaponEnergy(saveData.currentSWEnergy);
            playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().SetMaxWeaponEnergy(saveData.maxSWEnergy);
            playerObject.GetComponent<PlayerArtifactCollection>().SetArtifactsNumbersFromSave(saveData.currentArtifactNum, saveData.maxArtifactNum);
            playerObject.GetComponentInChildren<PlayerBasicShooting>().SetBaseShootingState(saveData.currentBaseBulletState);
            CameraManager.instance.SetCurrentCamera(saveData.currentCameraIndex);
            EnemyManager.instance.SetEnemyIDs(saveData.enemyIDs);
            EnemyManager.instance.SetBossEnemyIDs(saveData.bossEnemyIDs);
            StrayCollectableManager.Instance.SetCollectableIDs(saveData.collectablesIDs);
            MapRoomManager.instance.SetOpenRoomNames(saveData.openRoomNames);


        }
        else
        {
            Debug.Log("No Save");
        }

    }
}
