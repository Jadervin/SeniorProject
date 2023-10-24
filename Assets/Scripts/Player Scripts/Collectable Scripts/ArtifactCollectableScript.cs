using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactCollectableScript : Collectables
{
    


    public override void Interact()
    {
        if (player.gameObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded() > 0)
        {
            player.gameObject.GetComponent<PlayerArtifactCollection>().IncreaseArtifactCount();
        }
    }

}
