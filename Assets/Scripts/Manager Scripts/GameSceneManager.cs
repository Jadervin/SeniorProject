using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    GamePlaying,
    /*PlayerDead,*/
    Paused,
    Rebinding,
    MapOpen,
    Saving,
    GameOver,
}

public class GameSceneManager : MonoBehaviour
{
    

    public static GameSceneManager Instance { get; private set; }

    

    [SerializeField] private GameStates gameState;
    

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameState = GameStates.GamePlaying;

        
    }

    

    /*public bool IsGameOver()
    {
        return gameState == GameStates.GameOver;
    }*/

    /*public void SetGameStateToDeath()
    {
        gameState = GameStates.GameOver;
    }*/

    public GameStates GetGameState() { return gameState; }

    public void SetGameState(GameStates nextGameState)
    {
        gameState = nextGameState;
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
