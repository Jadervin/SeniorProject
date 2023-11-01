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
    [SerializeField] private List<string> enemiesWithItemsidsList;

    private int[] bossEnemiesWithItemsIndexes;
    [SerializeField] private List<int> bossEnemiesWithItemsIndexList;

    private string[] enemyidsFromSave = new string[0];
    [SerializeField] private List<string> enemyidsFromSaveList = new List<string>();

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
        enemiesWithItemsidsList = new List<string>();
        enemyidsFromSaveList = new List<string>();

        for (int i = 0; i < regularEnemyArray.Length; i++)
        {
            regularEnemyList.Add(regularEnemyArray[i].transform.parent.gameObject);
        }
        for (int i = 0; i < bossEnemyArray.Length; i++)
        {
            bossEnemyList.Add(bossEnemyArray[i].transform.parent.gameObject);
        }

        if (enemyidsFromSave != null /*enemiesItemIndexesFromSaveList.IsEmpty() == false*/)
        {
            CheckEnemyIndexes();
        }

        EnemyScript.OnAnyEnemyDefeated += EnemyScript_OnAnyEnemyDefeated;
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
                    if(enemiesWithItemsidsList.Contains(regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID()) == false)
                    {
                        enemiesWithItemsidsList.Add(regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID());
                    }
                }
            }
        }

        enemiesWithItemsIDs = new string[enemiesWithItemsidsList.Count];

        for(int i = 0; i < enemiesWithItemsIDs.Length; i++)
        {
            enemiesWithItemsIDs[i] = enemiesWithItemsidsList[i];

            
        }
    }


    private void CheckBossEnemyLists(/*EnemyScript.OnEnemyDefeatedEventArgs enemyObject*/)
    {

    }

    public string[] GetEnemyIDs()
    {
        return enemiesWithItemsIDs;
    }

    public void SetEnemyIDs(string[] indexes)
    {
        enemyidsFromSave = indexes;

        for (int i = 0; i < enemyidsFromSave.Length; i++)
        {
            enemyidsFromSaveList.Add(indexes[i]);
        }
    }

    private void CheckEnemyIndexes()
    {
        for (int j = 0; j < enemyidsFromSave.Length; j++)
        {
            for (int i = 0; i < regularEnemyList.Count; i++)
            {
                if (enemyidsFromSave[j] == regularEnemyList[i].GetComponentInChildren<EnemyScript>().GetEnemyID())
                {
                    regularEnemyList[i].SetActive(false);
                    //regularEnemyList.RemoveAt(i);
                }
            }
        }
        regularEnemyArray = FindObjectsOfType<EnemyScript>();
        regularEnemyList.Clear();
        for (int i = 0; i < regularEnemyArray.Length; i++)
        {
            regularEnemyList.Add(regularEnemyArray[i].transform.parent.gameObject);
        }
    }
}
