using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Vector2Int coords;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] bool _isOffset;

    public bool isBlocked = false;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetBlocked(bool blocked)
    {
        isBlocked = blocked;
    }
    public void UpdateBlockedStatus()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0f, LayerMask.GetMask("Enemy", "Player"));
        isBlocked = hit != null;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 0.5f); // This draws the area being checked
    // }
}
