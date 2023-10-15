using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public enum GameState
    {
        GamePlaying,
        /*PlayerDead,*/
        GameOver,
    }

    public static GameSceneManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    private GameState gameState;
    private bool isGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameState = GameState.GamePlaying;

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
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }

    }


    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public void SetGameStateToDeath()
    {
        gameState = GameState.GameOver;
    }


    /*
    public GameState GetGameState()
    {
        return gameState;
    }*/

    /*
    public bool IsThePlayerDead()
    {
        return gameState == GameState.PlayerDead;
    }*/
}
