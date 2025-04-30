using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    private Dictionary<Vector2Int, Tile> _tiles;

    public Pathfinder(Dictionary<Vector2Int, Tile> tiles)
    {
        _tiles = tiles;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Vector2Int>();

        openList.Add(new Node(start, null, 0, Vector2Int.Distance(start, goal)));

        while (openList.Count > 0)
        {
            var current = openList.OrderBy(n => n.F).First();
            if (current.Position == goal)
            {
                return ReconstructPath(current);
            }

            openList.Remove(current);
            closedList.Add(current.Position);

            foreach (var neighbor in GetNeighbors(current.Position))
            {
                if (closedList.Contains(neighbor)) continue;
                if (!_tiles.ContainsKey(neighbor) || _tiles[neighbor].isBlocked) continue;

                float g = current.G + 1;
                float h = Vector2.Distance(neighbor, goal);

                var existing = openList.FirstOrDefault(n => n.Position == neighbor);
                if (existing == null)
                {
                    openList.Add(new Node(neighbor, current, g, h));
                }
                else if (g < existing.G)
                {
                    existing.G = g;
                    existing.Parent = current;
                }
            }
        }

        return null; // No path
    }

    private List<Vector2Int> ReconstructPath(Node node)
    {
        var path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        return new List<Vector2Int>
        {
            pos + Vector2Int.up,
            pos + Vector2Int.down,
            pos + Vector2Int.left,
            pos + Vector2Int.right
        };
    }
    public List<Vector2Int> FindTopToBottomPath()
    {
        int maxY = (int)_tiles.Keys.Max(v => v.y);
        int minY = (int)_tiles.Keys.Min(v => v.y);

        var starts = _tiles.Keys.Where(v => v.y == minY && !_tiles[v].isBlocked);
        var goals = _tiles.Keys.Where(v => v.y == maxY && !_tiles[v].isBlocked);

        List<Vector2Int> shortestPath = null;

        foreach (var start in starts)
        {
            foreach (var goal in goals)
            {
                var path = FindPath(start, goal);
                if (path != null && (shortestPath == null || path.Count < shortestPath.Count))
                {
                    shortestPath = path;
                }
            }
        }

        return shortestPath;
    }

}
