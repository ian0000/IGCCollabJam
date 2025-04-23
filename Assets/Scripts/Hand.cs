using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GridManager _gridManager;
    [SerializeField] Card _cardPrefab;
    [SerializeField] int _cardCount;
    [SerializeField] float _drawWaitTime;
    [SerializeField] List<Card> _cards;
    [SerializeField] List<CardObject> _deck;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        Card.cardPlayed += CardPlayed;
        GameManager.turnStarted += EnableCards;
    }

    void EndTurn()
    {
        _cards.ForEach(c => c.enabled = false);
        _gameManager.ChangeState(GameState.EnemyTurn);
    }

    void EnableCards()
    {
        _cards.ForEach(c => c.enabled = true);
    }

    public void DrawCards()
    {
        StartCoroutine(DrawCardsRoutine());
    }

    IEnumerator DrawCardsRoutine()
    {
        for (int i = _cards.Count; i < _cardCount; i++)
        {
            if (_deck.Count == 0) break;

            yield return StartCoroutine(DrawCard());
        }

        _gameManager.ChangeState(GameState.EnemySpawnUnits);
    }

    IEnumerator DrawCard()
    {
        yield return new WaitForSeconds(_drawWaitTime);
        var card = Instantiate(_cardPrefab, transform);
        var randint = Random.Range(0, _deck.Count);
        var cardObj = _deck[randint];
        card.Init(_gridManager, cardObj);
        card.enabled = false;
        _cards.Add(card);
        _deck.Remove(card.CardObject);
    }

    void CardPlayed(Card card)
    {
        _deck.Add(card.CardObject);
        _cards.Remove(card);
        Destroy(card.gameObject);

        EndTurn();
    }
}
