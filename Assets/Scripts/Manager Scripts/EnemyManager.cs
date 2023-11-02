using SuperTiled2Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;


    private EnemyScript[] regularEnemyArray;
    private BossEnemyScript[] bossEnemyArray;
    [SerializeField] private List<GameObject> regularEnemyList;
    [SerializeField] private List<GameObject> bossEnemyList;

    private string[] enemiesWithItemsIDs;
    [SerializeField] private List<string> enemiesWithItemsIdsList;

    private string[] bossEnemiesWithItemIDs;
    [SerializeField] private List<string> bossEnemiesWithItemsIdsList;

    private string[] enemyIDsFromSave = new string[0];
    [SerializeField] private List<string> enemyIdsFromSaveList = new List<string>();

    private string[] bossEnemyIDsFromSave = new string[0];
    [SerializeField] private List<string> bossEnemyIdsFromSaveList = new List<string>();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        regularEnemyArray = FindObjectsOfType<EnemyScript>();
        bossEnemyArray = FindObjectsOfType<BossEnemyScript>();

        regularEnemyList = new List<GameObject>();
        bossEnemyList = new List<GameObject>();
        enemiesWithItemsIdsList = new List<string>();
        //enemyIdsFromSaveList = new List<string>();

        for (int i = 0; i < regularEnemyArray.Length; i++)
        {
            regularEnemyList.Add(regularEnemyArray[i].transform.parent.gameObject);
        }
        for (int i = 0; i < bossEnemyArray.Length; i++)
        {
            bossEnemyList.Add(bossEnemyArray[i].transform.parent.gameObject);
        }

        //Debug.Log(enemyIdsFromSave);

        if (enemyIDsFromSave.Length > 0)
        {
            //Debug.Log("Checking Regular Enemies");
            CheckEnemyIDs();
        }

        //Debug.Log(bossEnemyIdsFromSave);

        if (bossEnemyIDsFromSave.Length > 0)
        {
            //Debug.Log("Checking Boss Enemies");
            CheckBossEnemyIDs();
        }

        EnemyScript.OnAnyEnemyDefeated += EnemyScript_OnAnyEnemyDefeated;
        BossEnemyScript.OnAnyBossEnemyDefeated += BossEnemyScript_OnAnyBossEnemyDefeated;
    }

    private void BossEnemyScript_OnAnyBossEnemyDefeated(object sender, BossEnemyScript.OnBossEnemyDefeatedEventArgs e)
    {
        CheckBossEnemyLists(e);
    }

    private void EnemyScript_OnAnyEnemyDefeated(object sender, EnemyScript.OnEnemyDefeatedEventArgs e)
    {
        CheckEnemyLists(e);
    }

    private void CheckEnemyLists(EnemyScript.OnEnemyDefeatedEventArgs enemyObject)
    {
        for (int i = 0; i < regularEnemyList.Count; i++)
        {
            if(enemyObject.enemyParent == regularEnemyList[i])
            {
                if(enemyObject.itemToSpawn != null)
                {
                    if(enemiesWithItemsIdsList.Contains(regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID()) == false)
                    {
                        enemiesWithItemsIdsList.Add(regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID());
                    }
                }
            }
        }

        enemiesWithItemsIDs = new string[enemiesWithItemsIdsList.Count];

        for(int i = 0; i < enemiesWithItemsIDs.Length; i++)
        {
            enemiesWithItemsIDs[i] = enemiesWithItemsIdsList[i];

            
        }
    }


    private void CheckBossEnemyLists(BossEnemyScript.OnBossEnemyDefeatedEventArgs bossEnemyObject)
    {
        for (int i = 0; i < bossEnemyList.Count; i++)
        {
            if (bossEnemyObject.enemyParent == bossEnemyList[i])
            {
                if (bossEnemyObject.itemToSpawn != null)
                {
                    if (bossEnemiesWithItemsIdsList.Contains(bossEnemyList[i].GetComponentInChildren<BossEnemyScript>().GetBossEnemyID()) == false)
                    {
                        bossEnemiesWithItemsIdsList.Add(bossEnemyList[i].GetComponentInChildren<BossEnemyScript>().GetBossEnemyID());
                    }
                }
            }
        }

        bossEnemiesWithItemIDs = new string[bossEnemiesWithItemsIdsList.Count];

        for (int i = 0; i < bossEnemiesWithItemIDs.Length; i++)
        {
            bossEnemiesWithItemIDs[i] = bossEnemiesWithItemsIdsList[i];


        }
    }

    public string[] GetEnemyIDs()
    {
        return enemiesWithItemsIDs;
    }

    public string[] GetBossEnemyIDs()
    {
        return bossEnemiesWithItemIDs;
    }

    public void SetEnemyIDs(string[] ids)
    {
        enemyIDsFromSave = ids;

        for (int i = 0; i < enemyIDsFromSave.Length; i++)
        {
            enemyIdsFromSaveList.Add(ids[i]);
        }
    }


    public void SetBossEnemyIDs(string[] ids)
    {
        bossEnemyIDsFromSave = ids;

        for (int i = 0; i < bossEnemyIDsFromSave.Length; i++)
        {
            bossEnemyIdsFromSaveList.Add(ids[i]);
        }
    }

    private void CheckEnemyIDs()
    {
        for (int j = 0; j < enemyIDsFromSave.Length; j++)
        {
            for (int i = 0; i < regularEnemyList.Count; i++)
            {
                if (enemyIDsFromSave[j] == regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID())
                {
                    regularEnemyList[i].SetActive(false);
                    //regularEnemyList.RemoveAt(i);
                }
            }
        }

        //Debug.Log("regularEnemyArray.Length: " + regularEnemyArray.Length);
        regularEnemyArray = FindObjectsOfType<EnemyScript>();
        //Debug.Log("regularEnemyArray.Length: " + regularEnemyArray.Length);

        regularEnemyList.Clear();
        for (int i = 0; i < regularEnemyArray.Length; i++)
        {
            regularEnemyList.Add(regularEnemyArray[i].transform.parent.gameObject);
        }
    }



    private void CheckBossEnemyIDs()
    {
        for (int j = 0; j < bossEnemyIDsFromSave.Length; j++)
        {
            for (int i = 0; i < bossEnemyList.Count; i++)
            {
                if (bossEnemyIDsFromSave[j] == bossEnemyList[i].GetComponentInChildren<BossEnemyScript>().GetBossEnemyID())
                {
                    bossEnemyList[i].SetActive(false);
                    //regularEnemyList.RemoveAt(i);
                }
            }


        }
        bossEnemyArray = FindObjectsOfType<BossEnemyScript>();
        bossEnemyList.Clear();
        for (int i = 0; i < bossEnemyArray.Length; i++)
        {
            bossEnemyList.Add(bossEnemyArray[i].transform.parent.gameObject);
        }


    }
}
