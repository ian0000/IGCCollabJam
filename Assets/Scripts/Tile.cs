using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    public Vector2Int coords;
    public bool Blocked
    {
        get { return _occupier != null; }
    }
    public GameObject Occupier
    {
        get { return _occupier; }
    }

    [SerializeField] LayerMask layerMask;

    BoxCollider2D _boxCollider;
    GameObject _occupier;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool Contains(Vector2 pos)
    {
        return _boxCollider.bounds.Contains(pos);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (layerMask.LayerInMask(collider.gameObject.layer))
        {
            _occupier = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (layerMask.LayerInMask(collider.gameObject.layer))
        {
            _occupier = null;
        }
    }
}
