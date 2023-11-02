using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCollectableManager : MonoBehaviour
{
    public static StrayCollectableManager Instance;

    /*[SerializeField] */private Collectables[] collectablesArray;
    [SerializeField] private List<GameObject> collectablesList;

    /*[SerializeField] */private string[] collectablesIDs;
    [SerializeField] private List<string> collectablesIdsList;

    /*[SerializeField] */private string[] collectablesIDsFromSave = new string[0];
    [SerializeField] private List<string> collectablesIdsFromSaveList = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        collectablesArray = FindObjectsOfType<Collectables>();

        collectablesList = new List<GameObject>();
        collectablesIdsList = new List<string>();

        for (int i = 0; i < collectablesArray.Length; i++)
        {
            collectablesList.Add(collectablesArray[i].transform.gameObject);
        }


        if (collectablesIDsFromSave.Length > 0)
        {
            //Debug.Log("Checking Regular Enemies");
            CheckCollectablesIDs();
        }

        Collectables.OnCollectableGet += Collectables_OnCollectableGet;

    }

    private void Collectables_OnCollectableGet(object sender, Collectables.OnCollectableGetEventArgs e)
    {
        CheckCollectablesList(e);
    }


    private void CheckCollectablesList(Collectables.OnCollectableGetEventArgs collectableObject)
    {
        for (int i = 0; i < collectablesList.Count; i++)
        {
            if (collectableObject.collectable == collectablesList[i])
            {
                
                if (collectablesIdsList.Contains(collectablesList[i].GetComponent<Collectables>().GetCollectableID()) == false)
                {
                    collectablesIdsList.Add(collectablesList[i].GetComponent<Collectables>().GetCollectableID());
                    collectablesList.RemoveAt(i);
                }
                
            }
        }



        collectablesIDs = new string[collectablesIdsList.Count];

        for (int i = 0; i < collectablesIDs.Length; i++)
        {
            collectablesIDs[i] = collectablesIdsList[i];


        }

        collectablesArray = new Collectables[collectablesList.Count];

        if (collectablesArray.Length > 0)
        {
            for (int i = 0; i < collectablesArray.Length; i++)
            {
                collectablesArray[i] = collectablesList[i].GetComponent<Collectables>();


            }
        }



    }

    public string[] GetCollectableIDs()
    {
        return collectablesIDs;
    }

    public void SetCollectableIDs(string[] ids)
    {
        collectablesIDsFromSave = ids;

        for (int i = 0; i < collectablesIDsFromSave.Length; i++)
        {
            collectablesIdsFromSaveList.Add(ids[i]);
        }
    }

    private void CheckCollectablesIDs()
    {
        for (int j = 0; j < collectablesIDsFromSave.Length; j++)
        {
            for (int i = 0; i < collectablesList.Count; i++)
            {
                if (collectablesIDsFromSave[j] == collectablesList[i].GetComponent<Collectables>().GetCollectableID())
                {
                    collectablesList[i].SetActive(false);
                    Destroy(collectablesList[i]);
                    collectablesList.RemoveAt(i);
                }
            }
        }
        
        //Debug.Log("collectablesArray.Length: " + collectablesArray.Length);
        collectablesArray = FindObjectsOfType<Collectables>();
        //Debug.Log("collectablesArray.Length: " + collectablesArray.Length);
        collectablesList.Clear();
        //Debug.Log("collectablesList.Count: " + collectablesList.Count);
        for (int i = 0; i < collectablesArray.Length; i++)
        {
            collectablesList.Add(collectablesArray[i].gameObject);
        }
    }
}
