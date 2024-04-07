using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private const string FADE_OUT = "FadeOut";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {

            GameInput.Instance.GetPlayerInputActions().Disable();
            animator.SetTrigger(FADE_OUT);

            //LoadWinLevel();

        }
    }

    public void OnFadeComplete()
    {
        LoadWinLevel();
    }

    private void LoadWinLevel()
    {
        Loader.Load(Loader.GameScenes.WinScene);

    }
}
