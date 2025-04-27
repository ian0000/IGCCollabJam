using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedManager : MonoBehaviour {
    public static SeedManager Instance;

    public int currentSeeds { get; private set; }
    public event Action onSeedsChanged;

    [SerializeField] int maxSeeds = 5;
    private int turnCounter = 0;
    private const int turnsToReplenish = 3;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        currentSeeds = maxSeeds;
        Card.cardPlayed += OnCardPlayed;
        GameManager.turnEnded += AdvanceTurn;
        onSeedsChanged?.Invoke();
    }

    void OnDestroy() {
        Card.cardPlayed -= OnCardPlayed;
    }

    public int MaxSeeds
    {
        get
        {
            return maxSeeds;
        }
        set
        {
            maxSeeds = value;
            onSeedsChanged?.Invoke();
        }
    }

    public bool SpendSeeds(int amount) {
        if (amount > currentSeeds)
            return false;

        currentSeeds -= amount;
        onSeedsChanged?.Invoke();
        return true;
    }

    void OnCardPlayed(Card card) {
        int cost = card.GetSeedCost();

        if (cost > currentSeeds) {
            Debug.Log("Not enough seeds to play this card.");
            return;
        }

        currentSeeds -= cost;
        onSeedsChanged?.Invoke();
        Debug.Log($"Seeds remaining: {currentSeeds}");
    }

    void AdvanceTurn() {
        Debug.Log($"Seed Manager Count {turnCounter}");
        turnCounter++;
        if (turnCounter >= turnsToReplenish) {
            currentSeeds = maxSeeds;
            turnCounter = 0;
            onSeedsChanged?.Invoke();
            Debug.Log("Seeds replenished!");
        }
    }

    public bool CanPlayCard(Card card) {
        return card.GetSeedCost() <= currentSeeds;
    }
}
