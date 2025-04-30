using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    public Vector2Int coords;
    public bool isBlocked = false;

    [SerializeField] LayerMask layerMask;

    BoxCollider2D _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector2 pos)
    {
        return _boxCollider.bounds.Contains(pos);
    }

// TODO: Not triggering
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerInMask(collider.gameObject.layer, layerMask))
        {
            isBlocked = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerInMask(collider.gameObject.layer, layerMask))
        {
            isBlocked = false;
        }
    }

    bool LayerInMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 0.5f); // This draws the area being checked
    // }
}
