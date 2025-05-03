using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    class Node
    {
        public Tile tile;
        public int startDistance;
        public int endDistance;
        public int totalDistance => startDistance + endDistance;
        public Node prevNode;
    }

    /// <summary>
    /// Find path between tiles using A* using Manhattan distance.
    /// </summary>
    /// <param name="start">Start tile</param>
    /// <param name="goal">Target tile</param>
    /// <returns>Shortest list of tiles to the goal tile. List is in order from the start tile (exclusive) to the goal tile (inclusive).</returns>
    public static List<Tile> FindPath(Tile start, Tile goal)
    {
        var openList = new List<Node> { new Node { tile = start, startDistance = 0 } };

        while (openList.Count > 0)
        {
            // Choose shortest path to check first
            var current = openList.OrderBy(n => n.totalDistance).First();
            openList.Remove(current);

            // Get neighbors that aren't blocked and aren't where you just came from
            var freeNeighbors = GridManager.Instance.GetNeighbors(current.tile)
                .Where(t => !t.blocked)
                .Where(t => t != current.prevNode?.tile);
            foreach (var neighbor in freeNeighbors)
            {
                var node = new Node
                {
                    tile = neighbor,
                    startDistance = current.startDistance + 1,
                    endDistance = CalculateDistance(neighbor.coords, goal.coords),
                    prevNode = current,
                };

                // If goal reached, stop searching
                if (node.tile == goal)
                    return ReconstructPath(node);

                var existingNode = openList.SingleOrDefault(n => n.tile == node.tile);
                // If not already in openList, add it
                if (existingNode == null)
                {
                    openList.Add(node);
                }
                // If already in openList but with longer distance, replace existing node with this node
                else if (existingNode.totalDistance > node.totalDistance)
                {
                    openList.Remove(existingNode);
                    openList.Add(node);
                }
            }
        }
        return null; // No path
    }

    // Manhattan distance between two positions
    public static int CalculateDistance(Vector2Int from, Vector2Int to)
    {
        return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y);
    }

    // Return the Node's prevNode path in List form
    static List<Tile> ReconstructPath(Node node)
    {
        var path = new List<Tile>();
        while (node.prevNode != null)
        {
            path.Add(node.tile);
            node = node.prevNode;
        }
        path.Reverse();
        return path;
    }
}
