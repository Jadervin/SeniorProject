using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {
            Loader.Load(Loader.GameScenes.WinScene);
        }
    }
}
