using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenuUI : MonoBehaviour
{
    public static SaveMenuUI Instance { get; private set; }


    [SerializeField] private Button saveButton;

    [SerializeField] private Button cancelButton;

    [SerializeField] private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        saveButton.onClick.AddListener(() => {
            //Click
            GameSceneManager.Instance.SetGameState(GameStates.GamePlaying);
            ToggleTimePause();
            Hide();
        });

        cancelButton.onClick.AddListener(() => {
            //Click
            GameSceneManager.Instance.SetGameState(GameStates.GamePlaying);
            ToggleTimePause();
            Hide();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        saveButton.Select();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }


    public void ToggleTimePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            GameSceneManager.Instance.SetGameState(GameStates.Saving);
            
        }
        else
        {
            Time.timeScale = 1f;
            GameSceneManager.Instance.SetGameState(GameStates.GamePlaying);
            
        }

    }

}
