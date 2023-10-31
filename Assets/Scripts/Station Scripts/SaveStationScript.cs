using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStationScript : MonoBehaviour
{

    [SerializeField] private GameObject saveCollisionMessage;
    //[SerializeField] private GameObject saveMenuUI;
    [SerializeField] private bool isOnSaveStation = false;

    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnShootPressed += GameInput_OnShootPressed;

        saveCollisionMessage.SetActive(false);
    }


    private void GameInput_OnShootPressed(object sender, EventArgs e)
    {
        if (isOnSaveStation == true)
        {
            SaveMenuUI.Instance.Show();
            SaveMenuUI.Instance.ToggleTimePause();
        }
    }


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
}
