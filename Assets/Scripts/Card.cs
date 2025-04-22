using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Card> cardPlayed;

    [SerializeField] GridManager _gridManager;
    [SerializeField] GameObject _plantStats, _cardPrefab;
    [SerializeField] Sprite _plantBackground, _trapBackground, _fertilizerBackground;
    [SerializeField] TextMeshProUGUI _seedText, _nameText, _descriptionText, _healthText, _attackText;
    [SerializeField] Image _portrait, _background;
    [SerializeField] float _displacement;

    GameObject _zoomCard;
    Vector2? _startPosition;
    CardObject _card;
    RectTransform _rectTransform;
    bool _dragging, _display;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(GridManager gridManager, CardObject card, bool display = true)
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

        _display = display;
    }

    public int GetSeedCost() {
        return _card.seedCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!SeedManager.Instance.CanPlayCard(this)) return;

        if (_zoomCard != null)
        {
            Destroy(_zoomCard);
        }
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
        if (!_dragging && _display && _zoomCard == null)
        {
            _startPosition = transform.position;
            _zoomCard = Instantiate(_cardPrefab, transform, false);
            _zoomCard.transform.position += new Vector3(0, _displacement);
            _zoomCard.transform.localScale = new Vector2(2, 2);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_dragging)
        {
            if (_zoomCard != null)
            {
                Destroy(_zoomCard);
            }
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
