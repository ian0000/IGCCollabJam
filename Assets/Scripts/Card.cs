using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static event Action<Card> cardPlayed;

    [SerializeField] GridManager _gridManager;
    [SerializeField] Vector2 _startPosition;
    RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(GridManager gridManager)
    {
        _gridManager = gridManager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var offset = new Vector2(_rectTransform.rect.width / 2, _rectTransform.rect.height / 2);
        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _startPosition;
        Debug.Log(Camera.main.ScreenToWorldPoint(eventData.position));
        var tile = _gridManager.GetTileAtPosition(Camera.main.ScreenToWorldPoint(eventData.position));
        if (tile)
        {
            Debug.Log($"Card {name} used on tile {tile.name}");
            cardPlayed.Invoke(this);
        }
    }
}
