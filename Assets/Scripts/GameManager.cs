using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public int currentTurn = 0;

    public EnemySpawn enemySpawner; // Drag and drop this in the Inspector
    public Hand hand;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindObjectOfType<EnemySpawn>();
        }

        ChangeState(gameState); // start on whatever it's set to in the inspector for testing purposes
    }

    public void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.PlayerDrawCards:
                Debug.Log("Player draws cards.");
                hand.DrawCards();
                break;

            case GameState.EnemySpawnUnits:
                Debug.Log("Spawning enemies for turn " + currentTurn);
                enemySpawner.SpawnEnemiesForTurn(currentTurn);
                break;

            case GameState.PlayerTurn:
                // Player turn started � advance seed turn
                SeedManager.Instance.AdvanceTurn();
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
