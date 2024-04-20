using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStationScript : MonoBehaviour
{

    [SerializeField] private GameObject saveCollisionMessage;
    //[SerializeField] private GameObject saveMenuUI;
    [SerializeField] private bool isOnSaveStation = false;
    //[SerializeField] private bool isSaved;
    [SerializeField] private GameObject saveCheckmarkSprite;

    

    // Start is called before the first frame update
    void Start()
    {
        //GameInput.Instance.OnShootPressed += GameInput_OnShootPressed;
        GameInput.Instance.OnMapPressed += GameInput_OnMapPressed;

        saveCollisionMessage.SetActive(false);

        SaveMenuUI.Instance.OnSaved += SaveMenuUI_OnSaved;
        saveCheckmarkSprite.SetActive(false);
    }

    private void SaveMenuUI_OnSaved(object sender, EventArgs e)
    {
        saveCheckmarkSprite.SetActive(true);
    }

    private void GameInput_OnMapPressed(object sender, EventArgs e)
    {
        if (isOnSaveStation == true && GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            SaveMenuUI.Instance.Show();
            SaveMenuUI.Instance.ToggleTimePause();
        }
    }

    /*private void GameInput_OnShootPressed(object sender, EventArgs e)
    {
        if (isOnSaveStation == true)
        {
            SaveMenuUI.Instance.Show();
            SaveMenuUI.Instance.ToggleTimePause();
        }
    }*/


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {
            isOnSaveStation = true;
            saveCollisionMessage.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnSaveStation = false;
        saveCollisionMessage.SetActive(false);

        /*if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {
            saveCollisionMessage.SetActive(false);
        }*/
    }

    public bool GetIsOnSaveStation()
    {
        return isOnSaveStation;
    }
}
