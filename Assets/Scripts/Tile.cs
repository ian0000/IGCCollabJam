using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public bool isBlocked = false;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
    public void SetBlocked(bool blocked)
    {
        isBlocked = blocked;
    }
    public void UpdateBlockedStatus()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0f, LayerMask.GetMask("Enemy"));
        isBlocked = hit != null;

    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 0.5f); // This draws the area being checked
    // }
}
