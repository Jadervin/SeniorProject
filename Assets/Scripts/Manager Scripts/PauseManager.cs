using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public static PauseManager instance {  get; private set; }


    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameInput.Instance.OnPausePressed += GameInput_OnPausePressed;
    }

    private void GameInput_OnPausePressed(object sender, EventArgs e)
    {
        TogglePauseGame();
    }


    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            GameSceneManager.Instance.SetGameState(GameStates.Paused);
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            GameSceneManager.Instance.SetGameState(GameStates.GamePlaying);
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }

    }


}
