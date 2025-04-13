using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Card> cardPlayed;

    [SerializeField] GridManager _gridManager;
    [SerializeField] float _focusDisplacementAmount;
    [SerializeField] TextMeshProUGUI _seedText, _nameText, _descriptionText, _healthText, _attackText;
    // [SerializeField] 

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
        _healthText.text = _card.health.ToString();
        _attackText.text = _card.attack.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
            Debug.Log($"Card {name} used on tile {tile.name}");
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
