using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Hand : MonoBehaviour
{
    [SerializeField] GridManager _gridManager;
    [SerializeField] GameObject _discardArea;
    [SerializeField] Deck _deck;
    [SerializeField] Button _endTurnButton;
    [SerializeField] Card _cardPrefab;
    [SerializeField] int _cardCount, _maxEndingCards;
    [SerializeField] float _drawWaitTime;
    [SerializeField] List<Card> _cards;
    [SerializeField] AudioClip _playedSound, _discardSound;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Card.cardPlayed += CardPlayed;
        Card.cardDiscarded += CardDiscarded;
        GameManager.turnStarted += EnableCards;
    }

    void EnableCards()
    {
        _discardArea.SetActive(true);
        _endTurnButton.gameObject.SetActive(true);
        _cards.ForEach(c => c.enabled = true);
        CheckEndTurnEnabled();
    }

    public void EndTurn()
    {
        if (_cards.Count <= _maxEndingCards)
        {
            _discardArea.SetActive(false);
            _endTurnButton.gameObject.SetActive(false);
            _cards.ForEach(c => c.enabled = false);
            GameManager.Instance.ChangeState(GameState.EnemyMovement);
        }
    }

    public void DrawCards()
    {
        _endTurnButton.interactable = false;
        StartCoroutine(DrawCardsRoutine());
    }

    IEnumerator DrawCardsRoutine()
    {
        for (int i = _cards.Count; i < _cardCount; i++)
        {
            if (_deck.Count == 0) break;

            yield return StartCoroutine(DrawCard());
        }

        GameManager.Instance.ChangeState(GameState.EnemySpawnUnits);
    }

    IEnumerator DrawCard()
    {
        yield return new WaitForSeconds(_drawWaitTime);
        var card = Instantiate(_cardPrefab, transform);
        var cardObj = _deck.Draw();
        card.Init(_gridManager, cardObj);
        card.enabled = false;
        _cards.Add(card);
        _deck.Remove(card.CardObject);
    }

    void CardPlayed(Card card)
    {
        _audioSource.PlayOneShot(_playedSound);
        RemoveCard(card);
    }

    void CardDiscarded(Card card)
    {
        _audioSource.PlayOneShot(_discardSound);
        RemoveCard(card);
    }

    void RemoveCard(Card card)
    {
        _deck.Add(card.CardObject);
        _cards.Remove(card);
        Destroy(card.gameObject);
        CheckEndTurnEnabled();
    }

    void CheckEndTurnEnabled()
    {
        _endTurnButton.interactable = _cards.Count <= _maxEndingCards;
    }
}
