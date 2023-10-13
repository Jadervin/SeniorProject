using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactCollectableScript : Collectables
{
    


    public override void Interact()
    {
        player.gameObject.GetComponent<PlayerArtifactCollection>().IncreaseArtifactCount();
    }

}
