using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArtifactCollection : MonoBehaviour
{

    [SerializeField] private int artifactsCollected;
    [SerializeField] private int artifactsNeeded;
    //[SerializeField] private string ARTIFACTTAG = "Artifact";

    public event EventHandler<OnPlayerCollectsArtifactEventArgs> OnPlayerCollectsArtifact;
    public class OnPlayerCollectsArtifactEventArgs:EventArgs
    {
        public int currentArtifactNum;
    }


    public event EventHandler<OnLoadingSaveEventArgs> OnPlayerLoadingSave;
    public class OnLoadingSaveEventArgs:EventArgs 
    {
        
        public int currentArtifactNum;
        public int neededArtifactNum;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveSystem.SaveFileCheck() == false)
        {
            artifactsCollected = 0;
        }

        //If there is multiple planet totems, this may need to be changed
        PlanetTotemScript.OnPlayerStartingWorld += PlanetTotemScript_OnPlayerStartingWorld;
        PlanetTotemScript.OnPlayerHasAllArtifacts += PlanetTotemScript_OnPlayerHasAllArtifacts;
        
    }

    

    private void PlanetTotemScript_OnPlayerStartingWorld(object sender, PlanetTotemScript.OnSendingArtifactNumberEventArgs e)
    {
        artifactsNeeded = e.artifactsNeeded;
    }

    private void PlanetTotemScript_OnPlayerHasAllArtifacts(object sender, System.EventArgs e)
    {
        artifactsNeeded = 0;
        artifactsCollected = 0;
    }


    public int GetCurrentArtifactsCollected()
    {
        return artifactsCollected;
    }

    public int GetArtifactsNeeded()
    {
        return artifactsNeeded;
    }

    public void SetArtifactsNumbersFromSave(int artifactsCollected, int artifactsNeeded)
    {
        this.artifactsCollected = artifactsCollected;
        this.artifactsNeeded = artifactsNeeded;

        OnPlayerLoadingSave?.Invoke(this, new OnLoadingSaveEventArgs
        {
            currentArtifactNum = artifactsCollected,
            neededArtifactNum = artifactsNeeded,
        });

    }


/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ARTIFACTTAG))
        {
            artifactsCollected++;
        }
    }*/

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ARTIFACTTAG) && artifactsNeeded != 0)
        {
            collision.gameObject.SetActive(false);
            IncreaseArtifactCount();

            Destroy(collision.gameObject);
        }
    }
*/
    public void IncreaseArtifactCount()
    {
        artifactsCollected++;

        OnPlayerCollectsArtifact?.Invoke(this, new OnPlayerCollectsArtifactEventArgs
        {
            currentArtifactNum = artifactsCollected,
        });
    }
}
