using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2Int currentTilePos;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] int _stepsPerMove = 2;
    List<Vector2Int> _currentPath;
    int _currentPathIndex;
    bool _isMoving = false;

    public void Init(Vector2Int startPosition)
    {
        currentTilePos = startPosition;
    }

    void Start()
    {
        EnemyManager manager = FindObjectOfType<EnemyManager>();
        if (manager != null)
        {
            manager.RegisterEnemy(this);
        }
    }

    public IEnumerator MoveChunk()
    {
        yield return WaitThenMove(); // just yield the internal coroutine
    }

    IEnumerator WaitThenMove()
    {
        yield return new WaitForSeconds(1f);

        // Recalculate path from current position to bottom row
        var tiles = GridManager.Instance.GetTiles();
        var pathfinder = new Pathfinder(tiles);
        var newPath = FindTopToBottomPathFrom(currentTilePos, pathfinder, tiles);
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

    List<Vector2Int> FindTopToBottomPathFrom(Vector2Int start, Pathfinder pathfinder, Dictionary<Vector2Int, Tile> tiles)
    {
        int maxY = tiles.Keys.Max(v => v.y);

        var goals = tiles.Keys.Where(v => v.y == maxY && !tiles[v].blocked);

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

    IEnumerator FollowPathChunk(List<Vector2Int> pathChunk)
    {

        foreach (var pos in pathChunk)
        {
            MoveTo(pos);
            while (_isMoving) yield return null;
        }
    }

    void MoveTo(Vector2Int targetTilePos)
    {
        if (_isMoving) return;

        Tile tile = GridManager.Instance.GetTileAtPosition(targetTilePos);
        StartCoroutine(MoveToTile(tile.transform.position, targetTilePos));
    }

    IEnumerator MoveToTile(Vector3 targetPos, Vector2Int targetTilePos)
    {
        _isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = targetPos;
        currentTilePos = targetTilePos;

        _isMoving = false;
    }

    List<Vector2Int> FindTopToBottomPath(Pathfinder pathfinder, Dictionary<Vector2Int, Tile> tiles)
    {
        int maxY = tiles.Keys.Max(v => v.y);
        int minY = tiles.Keys.Min(v => v.y);

        var starts = tiles.Keys.Where(v => v.y == minY && !tiles[v].blocked);
        var goals = tiles.Keys.Where(v => v.y == maxY && !tiles[v].blocked);

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

    // 🆕 Start path from top to bottom and pause after first chunk
    public void MoveFromTopToBottom()
    {
        var tiles = GridManager.Instance.GetTiles();
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
}
