using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 CurrentTilePos;
    private GridManager _gridManager;
    private float _moveSpeed = 2f;
    private bool _isMoving = false;

    public void Init(GridManager gridManager)
    {
        _gridManager = gridManager;

    }

    public void MoveTo(Vector2 targetTilePos)
    {
        if (_isMoving) return;

        Tile tile = _gridManager.GetTileAtPosition(targetTilePos);
        if (tile == null)
        {
            Debug.LogWarning("Target tile doesn't exist");
            return;
        }

        StartCoroutine(MoveToTile(tile.transform.position, targetTilePos));
    }

    private System.Collections.IEnumerator MoveToTile(Vector3 targetPos, Vector2 targetTilePos)
    {
        _isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {

            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = targetPos;
        CurrentTilePos = targetTilePos;
        _isMoving = false;
    }
}
