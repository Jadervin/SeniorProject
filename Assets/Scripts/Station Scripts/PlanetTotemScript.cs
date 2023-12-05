using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTotemScript : MonoBehaviour
{
    //public static PlanetTotemScript instance {  get; private set; }
    [SerializeField] private List<GameObject> unlockableAreas;
    [SerializeField] private List<GameObject> bossMapSections;
    //[SerializeField] private List<GameObject> hiddenAreas;
    [SerializeField] private Collider2D switchCollider;
    [SerializeField] private int maxArtifacts = 3;
    [SerializeField] private bool completedTotem = false;

    public static event EventHandler<OnSendingArtifactNumberEventArgs> OnPlayerStartingWorld;
    public static event EventHandler OnPlayerHasAllArtifacts;

    public static void ResetStaticData()
    {
        OnPlayerStartingWorld = null;
        OnPlayerHasAllArtifacts = null;
    }

    public class OnSendingArtifactNumberEventArgs : EventArgs
    {
        public int artifactsNeeded;
    }

    private void Awake()
    {
        /*if(instance == null)
        {
            instance = this;
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        OnPlayerStartingWorld?.Invoke(this, new OnSendingArtifactNumberEventArgs
        {
            artifactsNeeded = maxArtifacts
        });
        */
        /*
        foreach (GameObject platform in hiddenAreas)
        {
            platform.SetActive(false);
        }*/

        foreach (GameObject wall in unlockableAreas)
        {
            wall.SetActive(true);
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag(PLAYERTAG) &&
            collision.gameObject.GetComponent<PlayerArtifactCollection>().GetCurrentArtifactsCollected() == maxArtifacts)
        {
            unlockableWall.SetActive(false);
            OnPlayerHasAllArtifacts?.Invoke(this, EventArgs.Empty);
        }

        if (collision.gameObject.CompareTag(PLAYERTAG) &&
            collision.gameObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded() == 0)
        {
            foreach (GameObject platform in unlockablePlatforms)
            {
                platform.SetActive(false);
            }

            OnPlayerStartingWorld?.Invoke(this, new OnSendingArtifactNumberEventArgs
            {
                artifactsNeeded = maxArtifacts
            });
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG) &&
            collision.gameObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded() == 0 &&
            completedTotem == false)
        {
            /*
            foreach (GameObject platform in hiddenAreas)
            {
                platform.SetActive(true);
            }
            */

            foreach(GameObject mapSection in bossMapSections)
            {
                mapSection.SetActive(true);
            }

            OnPlayerStartingWorld?.Invoke(this, new OnSendingArtifactNumberEventArgs
            {
                artifactsNeeded = maxArtifacts
            });
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG) &&
            collision.gameObject.GetComponent<PlayerArtifactCollection>().GetCurrentArtifactsCollected() >= maxArtifacts)
        {
            foreach (GameObject wall in unlockableAreas)
            {
                wall.SetActive(false);
            }
            completedTotem = true;
            OnPlayerHasAllArtifacts?.Invoke(this, EventArgs.Empty);
        }

        
    }
}
