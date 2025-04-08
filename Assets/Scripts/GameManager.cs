using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameState GameState;

    void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        ChangeState(GameState.PlayerDrawCards);
    }

    public void ChangeState(GameState newState) 
    {
        GameState = newState;
        switch (newState) 
        {
            case GameState.PlayerDrawCards:
                break;
            case GameState.EnemySpawnUnits:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState 
{
    PlayerDrawCards = 0,
    EnemySpawnUnits = 1,
    PlayerTurn = 2,
    EnemyTurn = 3,
    Victory = 4,
    Lose = 5
}
