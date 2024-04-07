using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {
            TutorialCheck.instance.SetPlayedTutorialCheck(true);
            Loader.Load(Loader.GameScenes.GameScene);
        }
    }
}
