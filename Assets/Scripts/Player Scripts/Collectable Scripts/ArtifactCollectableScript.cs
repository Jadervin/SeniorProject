using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactCollectableScript : Collectables
{

    [SerializeField] private float speed = 5f;

    private void Update()
    {
        //MOVE TO PLAYER
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public override void Interact()
    {
        if (player.gameObject.GetComponent<PlayerArtifactCollection>().GetArtifactsNeeded() > 0)
        {
            player.gameObject.GetComponent<PlayerArtifactCollection>().IncreaseArtifactCount();
        }
    }

}
