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

    // TODO: temp for testing
    [SerializeField] List<CardObject> _deck;

    void Start()
    {
        Card.cardPlayed += CardPlayed;
    }

    public void DrawCards()
    {
        _cards.All(c => c.enabled = false);
        StartCoroutine(DrawCardsRoutine());
    }

    IEnumerator DrawCardsRoutine()
    {
        for (int i = _cards.Count; i < _cardCount; i++)
        {
            yield return StartCoroutine(DrawCard());
        }

        _cards.All(c => c.enabled = true);
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
    }

    void CardPlayed(Card card)
    {
        _cards.Remove(card);
        Destroy(card.gameObject);
    }
}
