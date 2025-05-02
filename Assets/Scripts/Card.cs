using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Card> cardPlayed, cardDiscarded;

    [SerializeField] GridManager _gridManager;
    [SerializeField] GameObject _plantStats, _cardPrefab;
    [SerializeField] Sprite _plantBackground, _trapBackground, _fertilizerBackground;
    [SerializeField] TextMeshProUGUI _nameText, _descriptionText;
    [SerializeField] DisplayNumber _seedCount, _health, _attack;
    [SerializeField] Image _portrait, _background;
    [SerializeField] float _displacement;

    GameObject _zoomCard;
    Vector2? _startPosition;
    CardObject _cardObject;
    RectTransform _rectTransform;
    Image _image;
    bool _dragging, _display;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public CardObject CardObject
    {
        get { return _cardObject; }
    }

    public void Init(GridManager gridManager, CardObject card, bool display = true)
    {
        _gridManager = gridManager;
        _cardObject = card;
        _seedCount.SetNumber(_cardObject.seedCount);
        _nameText.text = _cardObject.cardName;
        _descriptionText.text = _cardObject.description;
        _portrait.sprite = _cardObject.portrait;
        _plantStats.SetActive(false);
        _background.sprite = _plantBackground;
        if (_cardObject.cardType == CardType.PLANT)
        {
            _background.sprite = _plantBackground;
            _plantStats.SetActive(true);
            _health.SetNumber(_cardObject.health);
            _attack.SetNumber(_cardObject.attack);
        }
        else if (_cardObject.cardType == CardType.TRAP)
        {
            _background.sprite = _trapBackground;
        }
        else if (_cardObject.cardType == CardType.FERTILIZER)
        {
            _background.sprite = _fertilizerBackground;
        }

        _display = display;
    }

    public int GetSeedCost() {
        return _cardObject.seedCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;

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
        _image.raycastTarget = true;

        // Check if dropped on discard
        if (eventData.pointerEnter?.tag == "Discard")
        {
            Debug.Log($"Card {name} discarded");
            cardDiscarded?.Invoke(this);
        }

        var tile = _gridManager.GetTileAtPosition(Camera.main.ScreenToWorldPoint(eventData.position));
        if (tile) 
        {
            // Check if player has enough seeds
            if (_cardObject.seedCount > SeedManager.Instance.currentSeeds) 
            {
                Debug.Log("Not enough seeds to play this card.");
                return;
            }
            if (tile.blocked)
            {
                Debug.Log("Tile already occupided");
                return;
            }
            Debug.Log($"Card {name} used on tile {tile.name}");
            var plant = Instantiate(_cardObject.plantPrefab, tile.transform.position, Quaternion.identity);
            plant.tilePosition = tile.transform.position;
            cardPlayed?.Invoke(this);
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
