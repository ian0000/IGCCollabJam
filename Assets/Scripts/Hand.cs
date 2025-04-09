using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GridManager _gridManager;
    [SerializeField] Card _cardPrefab;
    [SerializeField] List<Card> _cards;
    [SerializeField] int _cardCount;
    [SerializeField] float _drawWaitTime;

    void Start()
    {
        Card.cardPlayed += CardPlayed;
    }

    public void DrawCards()
    {
        StartCoroutine(DrawCardsRoutine());
    }

    IEnumerator DrawCardsRoutine()
    {
        for (int i = _cards.Count; i < _cardCount; i++)
        {
            yield return StartCoroutine(DrawCard());
        }
    }

    IEnumerator DrawCard()
    {
        yield return new WaitForSeconds(_drawWaitTime);
        var card = Instantiate(_cardPrefab, transform);
        card.Init(_gridManager);
        _cards.Append(card);
    }

    void CardPlayed(Card card)
    {
        _cards.Remove(card);
        Destroy(card.gameObject);
    }
}
