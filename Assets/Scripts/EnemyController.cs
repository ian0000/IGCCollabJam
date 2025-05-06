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
    [SerializeField] LayerMask plantLayer;
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
        var newPath = DetermineTarget();
        if (newPath?.Count > 0)
        {
            int steps = Mathf.Min(_stepsPerMove, newPath.Count);
            var pathChunk = newPath.GetRange(0, steps);

            yield return FollowPath(pathChunk);
        }
        Debug.LogWarning("No valid path found after rechecking.");
    }

    List<Tile> DetermineTarget()
    {
        var path = ShortestPathTo(GridManager.Instance.GetFreeTopRowTiles());
        if (path.Count > _stepsPerMove)
        {
            var occupiedTiles = GridManager.Instance.GetOccupiedTiles(plantLayer);
            if (occupiedTiles.Count > 0)
                path = ShortestPathTo(occupiedTiles);
        }
        return path;
    }

    /// <summary>
    /// Determines the shortest path to one of the targets and returns a path to it.
    /// </summary>
    /// <param name="targets">List of targetted tiles</param>
    /// <returns>Path to the closest target tile</returns>
    List<Tile> ShortestPathTo(List<Tile> targets)
    {
        List<Tile> shortestPath = null;
        foreach (var targetTile in targets)
        {
            var path = PathFinder.FindPath(currentTile, targetTile);
            if (shortestPath == null || path.Count < shortestPath.Count)
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
