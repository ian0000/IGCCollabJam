using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int _stepsPerMove = 2;
    public Vector2Int CurrentTilePos;
    private Vector2Int _previousTilePos;

    private GridManager _gridManager;
    private float _moveSpeed = 2f;
    private bool _isMoving = false;

    private List<Vector2Int> _currentPath;
    private int _currentPathIndex;
    public void Init(GridManager gridManager)
    {
        _gridManager = gridManager;

    }
    private void Start()
    {
        EnemyManager manager = FindObjectOfType<EnemyManager>();
        if (manager != null)
        {
            manager.RegisterEnemy(this);
        }
    }
    public void MoveTo(Vector2Int targetTilePos)
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

    private System.Collections.IEnumerator MoveToTile(Vector3 targetPos, Vector2Int targetTilePos)
    {
        _isMoving = true;
        _previousTilePos = CurrentTilePos;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {

            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = targetPos;
        CurrentTilePos = targetTilePos;

        var currentTile = _gridManager.GetTileAtPosition(CurrentTilePos);
        if (currentTile != null)
        {
            // currentTile.UpdateBlockedStatus();
        }
        var previousTile = _gridManager.GetTileAtPosition(_previousTilePos);
        if (previousTile != null)
        {
            // previousTile.UpdateBlockedStatus();
        }

        _isMoving = false;
    }

    public void FollowPath(List<Vector2Int> path)
    {
        StartCoroutine(FollowPathCoroutine(path));
    }

    private IEnumerator FollowPathCoroutine(List<Vector2Int> path)
    {
        foreach (var pos in path)
        {
            MoveTo(pos); // existing MoveTo(Vector2)
            while (_isMoving) yield return null;
        }
    }

    private List<Vector2Int> FindTopToBottomPath(Pathfinder pathfinder, Dictionary<Vector2Int, Tile> tiles)
    {
        int maxY = tiles.Keys.Max(v => v.y);
        int minY = tiles.Keys.Min(v => v.y);

        var starts = tiles.Keys.Where(v => v.y == minY && !tiles[v].isBlocked);
        var goals = tiles.Keys.Where(v => v.y == maxY && !tiles[v].isBlocked);

        List<Vector2Int> shortestPath = null;

        foreach (var start in starts)
        {
            foreach (var goal in goals)
            {
                var path = pathfinder.FindPath(start, goal);
                if (path != null && (shortestPath == null || path.Count < shortestPath.Count))
                {
                    shortestPath = path;
                }
            }
        }

        return shortestPath;
    }
    public void SetStepsPerMove(int steps)
    {
        _stepsPerMove = Mathf.Max(1, steps);
    }


    // 🆕 Start path from top to bottom and pause after first chunk
    public void MoveFromTopToBottom()
    {
        var tiles = _gridManager.GetTiles();
        var pathfinder = new Pathfinder(tiles);
        var path = FindTopToBottomPath(pathfinder, tiles);

        if (path != null)
        {
            _currentPath = path;
            _currentPathIndex = 0;
        }
        else
        {
            Debug.LogWarning("No valid path from top to bottom!");
        }
    }

    // 🆕 Call this externally to continue movement

    // 🆕 Partial path following
    private IEnumerator FollowPathChunk(List<Vector2Int> pathChunk)
    {

        foreach (var pos in pathChunk)
        {
            MoveTo(pos);
            while (_isMoving) yield return null;
        }
    }


    private IEnumerator WaitThenMove()
    {
        yield return new WaitForSeconds(1f); // ⏳ Wait 5 seconds

        // Recalculate path from current position to bottom row
        var tiles = _gridManager.GetTiles();
        var pathfinder = new Pathfinder(tiles);
        var newPath = FindTopToBottomPathFrom(CurrentTilePos, pathfinder, tiles);
        if (newPath != null && newPath.Count > 0)
        {
            _currentPath = newPath;
            _currentPathIndex = 0;

            int steps = Mathf.Min(_stepsPerMove, _currentPath.Count);
            var pathChunk = _currentPath.GetRange(_currentPathIndex, steps);
            _currentPathIndex += steps;

            yield return FollowPathChunk(pathChunk);
        }
        else
        {
            Debug.LogWarning("No valid path found after rechecking.");
        }
    }
    private List<Vector2Int> FindTopToBottomPathFrom(Vector2Int start, Pathfinder pathfinder, Dictionary<Vector2Int, Tile> tiles)
    {
        int maxY = (int)tiles.Keys.Max(v => v.y);

        var goals = tiles.Keys.Where(v => v.y == maxY && !tiles[v].isBlocked);

        List<Vector2Int> shortestPath = null;

        foreach (var goal in goals)
        {
            var path = pathfinder.FindPath(start, goal);
            if (path != null && (shortestPath == null || path.Count < shortestPath.Count))
            {
                shortestPath = path;
            }
        }

        return shortestPath;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
    public IEnumerator ContinueMovementCoroutine()
    {
        yield return WaitThenMove(); // just yield the internal coroutine
    }
}
