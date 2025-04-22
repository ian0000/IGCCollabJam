using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Card> cardPlayed;

    [SerializeField] GridManager _gridManager;
    [SerializeField] float _focusDisplacementAmount;
    [SerializeField] GameObject _plantStats;
    [SerializeField] Sprite _plantBackground, _trapBackground, _fertilizerBackground;
    [SerializeField] TextMeshProUGUI _seedText, _nameText, _descriptionText, _healthText, _attackText;
    [SerializeField] Image _portrait, _background;

    Vector2? _startPosition;
    CardObject _card;
    RectTransform _rectTransform;
    bool _dragging;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(GridManager gridManager, CardObject card)
    {
        _gridManager = gridManager;
        _card = card;
        _seedText.text = _card.seedCount.ToString();
        _nameText.text = _card.cardName;
        _descriptionText.text = _card.description;
        _portrait.sprite = _card.portrait;
        _plantStats.SetActive(false);
        _background.sprite = _plantBackground;
        if (_card.cardType == CardType.PLANT)
        {
            _background.sprite = _plantBackground;
            _plantStats.SetActive(true);
            _healthText.text = _card.health.ToString();
            _attackText.text = _card.attack.ToString();
        }
        else if (_card.cardType == CardType.TRAP)
        {
            _background.sprite = _trapBackground;
        }
        else if (_card.cardType == CardType.FERTILIZER)
        {
            _background.sprite = _fertilizerBackground;
        }
    }

    public int GetSeedCost() {
        return _card.seedCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!SeedManager.Instance.CanPlayCard(this)) return;

        _dragging = true;
        if (!_startPosition.HasValue)
        {
            _startPosition = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var offset = new Vector2(_rectTransform.rect.width / 2, _rectTransform.rect.height / 2);
        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();
        var tile = _gridManager.GetTileAtPosition(Camera.main.ScreenToWorldPoint(eventData.position));
        if (tile) 
        {
            // Check if player has enough seeds
            if (_card.seedCount > SeedManager.Instance.currentSeeds) 
            {
                Debug.Log("Not enough seeds to play this card.");
                return;
            }
            Debug.Log($"Card {name} used on tile {tile.name}");
            var plant = Instantiate(_card.plantPrefab, tile.transform.position, Quaternion.identity);
            plant.tilePosition = tile.transform.position;
            cardPlayed.Invoke(this);
        }
        _dragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_dragging)
        {
            _startPosition = transform.position;
            transform.position += new Vector3(0, _focusDisplacementAmount);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_dragging)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        if (_startPosition.HasValue)
        {
            transform.position = _startPosition.Value;
            _startPosition = null;
        }
    }
}
