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
        player.OnPlayerLoadingSave += Player_OnPlayerLoadingSave;

        /*artifactsCollected = 0;
        artifactsCollectedText.text = "0";
        artifactsNeededText.text = "/ 0";
*/

        if (SaveSystem.SaveFileCheck() == false)
        {
            artifactsCollected = 0;
            artifactsCollectedText.text = "0";
            artifactsNeededText.text = "/ 0";
        }
    }

    private void Player_OnPlayerLoadingSave(object sender, PlayerArtifactCollection.OnLoadingSaveEventArgs e)
    {
        ChangeArtifactNumbersFromSave(e);
    }

    private void Player_OnPlayerCollectsArtifact(object sender, PlayerArtifactCollection.OnPlayerCollectsArtifactEventArgs e)
    {
        ChangeCurrentArtifact(e);
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

    public void ChangeCurrentArtifact(PlayerArtifactCollection.OnPlayerCollectsArtifactEventArgs playerObject)
    {
        artifactsCollected = playerObject.currentArtifactNum;
        artifactsCollectedText.text = artifactsCollected.ToString();
    }


    public void ChangeArtifactNumbersFromSave(PlayerArtifactCollection.OnLoadingSaveEventArgs playerObject)
    {
        artifactsCollected = playerObject.currentArtifactNum;
        artifactsCollectedText.text = artifactsCollected.ToString();

        artifactsNeededText.text = "/ " + playerObject.neededArtifactNum.ToString();
    }

   
}
