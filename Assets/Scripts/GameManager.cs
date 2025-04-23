using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action turnStarted;

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

            case GameState.PlayCards:
                // Player turn started, increment turn counters
                turnStarted?.Invoke();
                Debug.Log("Card playing phase begins.");
                break;

            case GameState.EnemyTurn:
                Debug.Log("Enemies move and attack.");
                // TODO: Call enemy moving and attacking processes and move this state change to the end of it
                ChangeState(GameState.PlayerTurn);
                break;
            
            case GameState.PlayerTurn:
                Debug.Log("Player entities attack.");
                // TODO: Call player attacking processes and move this state change to the end of it
                SeedManager.Instance.AdvanceTurn();
                currentTurn++; // TODO: Should there be an end turn state?
                ChangeState(GameState.PlayCards);
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
    PlayerDrawCards,
    EnemySpawnUnits,
    PlayCards,
    EnemyTurn,
    PlayerTurn,
    Victory,
    Lose
}
