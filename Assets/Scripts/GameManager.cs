using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;
    public int currentTurn = 0;

    public EnemySpawn enemySpawner; // Drag and drop this in the Inspector
    public Hand hand;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (enemySpawner == null) {
            enemySpawner = FindObjectOfType<EnemySpawn>();
        }

        ChangeState(GameState.PlayerDrawCards);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState) {
            case GameState.PlayerDrawCards:
                Debug.Log("Player draws cards.");
                hand.DrawCards();
                break;

            case GameState.EnemySpawnUnits:
                Debug.Log("Spawning enemies for turn " + currentTurn);
                enemySpawner.SpawnEnemiesForTurn(currentTurn);
                break;

            case GameState.PlayerTurn:
                Debug.Log("Player's turn begins.");
                break;

            case GameState.EnemyTurn:
                Debug.Log("Enemy's turn begins.");
                break;

            case GameState.Victory:
                Debug.Log("Player wins!");
                break;

            case GameState.Lose:
                Debug.Log("Player loses.");
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
