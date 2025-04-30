using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Deck : MonoBehaviour
{
    [SerializeField] GameObject _lowerCardsGroup, _topCard;
    [SerializeField] List<CardObject> _cards;

    AudioSource _audioSource;

    public int Count
    {
        get { return _cards.Count; }
    }

    public void Remove(CardObject card)
    {
        _cards.Remove(card);
        CheckCards();
    }

    public void Add(CardObject card)
    {
        _cards.Add(card);
        CheckCards();
    }

    public CardObject Draw()
    {
        _audioSource.Play();
        var randint = Random.Range(0, _cards.Count);
        return _cards[randint];
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Card.cardPlayed += (Card _) => CheckCards();
        Card.cardDiscarded += (Card _) => CheckCards();
    }

    void CheckCards()
    {
        if (_cards.Count > 1)
        {
            _lowerCardsGroup.SetActive(true);
            _topCard.SetActive(true);
        }
        else if (_cards.Count == 1)
        {
            _lowerCardsGroup.SetActive(false);
            _topCard.SetActive(true);
        }
        else
        {
            _lowerCardsGroup.SetActive(false);
            _topCard.SetActive(false);
        }
    }
}
