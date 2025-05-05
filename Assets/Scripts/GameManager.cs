using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action turnStarted, turnEnded;

    public static GameManager Instance;
    public GameState gameState;
    public int currentTurn = 0;

    public EnemySpawn enemySpawner; // Drag and drop this in the Inspector
    public Hand hand;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
                Debug.Log("Card playing phase begins.");
                turnStarted?.Invoke();
                break;

            case GameState.EnemyMovement:
                Debug.Log("Enemies moving.");
                EnemyManager.Instance.MoveAllEnemies();
                break;
            
            case GameState.EnemyAttack:
                Debug.Log("Enemies attacking.");
                // TODO: Call enemy attacking processes and move this state change to the end of it
                ChangeState(GameState.PlayerAttack);
                break;
            
            case GameState.PlayerAttack:
                Debug.Log("Player entities attacking.");
                // TODO: Call player attacking processes and move this state change to the end of it
                ChangeState(GameState.EndTurn);
                break;
            
            case GameState.EndTurn:
                turnEnded?.Invoke();
                currentTurn++;
                // TODO: Added this here to close loop, but we may have some kind of coroutine
                // to handle end turn behavior and create delay before next draw phase
                ChangeState(GameState.PlayerDrawCards);
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
    EnemyMovement,
    EnemyAttack,
    PlayerAttack,
    EndTurn,
    Victory,
    Lose
}
