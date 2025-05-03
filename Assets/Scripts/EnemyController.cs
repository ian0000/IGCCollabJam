using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Tile currentTile;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] int _stepsPerMove = 2;
    List<Vector2Int> _currentPath;
    bool _isMoving = false;

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
        yield return Move(); // just yield the internal coroutine
    }

    IEnumerator Move()
    {
        // Calculate path from current position to top row
        var newPath = FindPathToTop();
        if (newPath?.Count > 0)
        {
            int steps = Mathf.Min(_stepsPerMove, newPath.Count);
            var pathChunk = newPath.GetRange(0, steps);

            yield return FollowPath(pathChunk);
        }
        Debug.LogWarning("No valid path found after rechecking.");
    }

    /// <summary>
    /// Finds the shortest path to a free top tile.
    /// </summary>
    /// <returns>Shortest tile path (see PathFinder.FindPath for more info on tile path).</returns>
    List<Tile> FindPathToTop()
    {
        var topTiles = GridManager.Instance.GetFreeTopRowTiles();
        List<Tile> shortestPath = null;
        foreach (var top in topTiles)
        {
            var path = PathFinder.FindPath(currentTile, top);
            if (shortestPath == null || path?.Count < shortestPath.Count)
            {
                shortestPath = path;
            }
        }

        return shortestPath;
    }

    IEnumerator FollowPath(List<Tile> path)
    {

        foreach (var tile in path)
        {
            MoveTo(tile);
            while (_isMoving) yield return null;
        }
    }

    void MoveTo(Tile tile)
    {
        if (_isMoving) return;
        StartCoroutine(MoveToTile(tile));
    }

    IEnumerator MoveToTile(Tile tile)
    {
        _isMoving = true;

        while (Vector2.Distance(transform.position, tile.transform.position) > 0.01f)
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, tile.transform.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = tile.transform.position;
        currentTile = tile;

        _isMoving = false;
    }
}
