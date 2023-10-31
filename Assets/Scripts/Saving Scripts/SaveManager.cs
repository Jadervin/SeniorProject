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

        playerObject = GameObject.FindGameObjectWithTag("Player");

    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Save()
    {
        SaveData saveData = new SaveData
        {
            playerLocation = playerObject.transform.position,
            currentHealth = playerObject.GetComponent<PlayerHealth>().GetCurrentHealth(),
            maxHealth = playerObject.GetComponent<PlayerHealth>().GetMaxHealth(),
            currentSWEnergy = playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().GetCurrentWeaponEnergy(),
            maxSWEnergy = playerObject.GetComponentInChildren<SpecialWeaponManagerScript>().GetMaxWeaponEnergy(),
            currentArtifactNum = playerObject.GetComponent<PlayerArtifactCollection>().GetCurrentArtifactsCollected(),
            maxArtifactNum= playerObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded(),
            currentBaseBulletState = playerObject.GetComponentInChildren<PlayerBasicShooting>().GetBaseShootingState()
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
            Debug.Log("Loaded");
            Debug.Log(saveString);


            SaveData saveData = JsonUtility.FromJson<SaveData>(saveString);
        }
        else
        {
            Debug.Log("No Save");
        }

    }
}
