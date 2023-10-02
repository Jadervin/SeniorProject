using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactCollectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI artifactsCollectedText;
    [SerializeField] private int artifactsCollected;
    [SerializeField] private TextMeshProUGUI artifactsNeededText;
    [SerializeField] private PlayerArtifactCollection player;



    // Start is called before the first frame update
    void Start()
    {

        //If there is multiple planet totems, this may need to be changed
        PlanetTotemScript.OnPlayerStartingWorld += PlanetTotemScript_OnPlayerStartingWorld;
        PlanetTotemScript.OnPlayerHasAllArtifacts += PlanetTotemScript_OnPlayerHasAllArtifacts;

        player.OnPlayerCollectsArtifact += Player_OnPlayerCollectsArtifact;
        artifactsCollected = 0;
        artifactsCollectedText.text = "0";
        artifactsNeededText.text = "/ 0";
    }

    private void Player_OnPlayerCollectsArtifact(object sender, System.EventArgs e)
    {
        ChangeCurrentArtifact();
    }

    private void PlanetTotemScript_OnPlayerStartingWorld(object sender, PlanetTotemScript.OnSendingArtifactNumberEventArgs e)
    {
        //artifactsNeededText.text = "/ " + e.artifactsNeeded.ToString();
        SetNeededArtifacts(e.artifactsNeeded.ToString());

    }

    private void PlanetTotemScript_OnPlayerHasAllArtifacts(object sender, System.EventArgs e)
    {
        /*
        artifactsCollectedText.text = "0";
        artifactsNeededText.text = "/ 0";
        */
        DeleteNeededArtifacts();
    }

    public void SetNeededArtifacts(string artifactsneededString)
    {
        artifactsNeededText.text = "/ " + artifactsneededString;
    }

    public void DeleteNeededArtifacts()
    {
        artifactsCollected = 0;
        artifactsCollectedText.text = "0";
        artifactsNeededText.text = "/ 0";
    }

    public void ChangeCurrentArtifact()
    {
        artifactsCollected++;
        artifactsCollectedText.text = artifactsCollected.ToString();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
