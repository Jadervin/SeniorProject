using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArtifactCollection : MonoBehaviour
{

    [SerializeField] private int artifactsCollected;
    [SerializeField] private int artifactsNeeded;
    [SerializeField] private string ARTIFACTTAG = "Artifact";

    public event EventHandler OnPlayerCollectsArtifact;

    // Start is called before the first frame update
    void Start()
    {
        artifactsCollected = 0;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentArtifactsCollected()
    {
        return artifactsCollected;
    }

    public int GetArtifactsNeeded()
    {
        return artifactsNeeded;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag(ARTIFACTTAG))
    //    {
    //        artifactsCollected++;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ARTIFACTTAG) && artifactsNeeded != 0)
        {
            collision.gameObject.SetActive(false);
            artifactsCollected++;

            OnPlayerCollectsArtifact?.Invoke(this, EventArgs.Empty);
        }
    }
}
