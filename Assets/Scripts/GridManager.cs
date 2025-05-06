using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public static Vector2Int[] Directions = new Vector2Int[] {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };

    [SerializeField] int _width, _height;
    [SerializeField] Tile _tilePrefab;
    [SerializeField] Transform _cam;

    Dictionary<Vector2Int, Tile> _tiles;
    int maxY;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _tiles = new Dictionary<Vector2Int, Tile>();
        foreach (var tile in GetComponentsInChildren<Tile>())
        {
            _tiles[tile.coords] = tile;
            tile.name = $"Tile {tile.coords.x}, {tile.coords.y}";
        }
        maxY = _tiles.Max(t => t.Key.y);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        foreach (var tile in _tiles.Values)
        {
            if (tile.Contains(pos))
            {
                return tile;
            }
        }
        return null;
    }

    public Tile[] GetNeighbors(Tile tile)
    {
        return Directions
            .Where(d => _tiles.ContainsKey(tile.coords + d))
            .Select(d => _tiles[tile.coords + d])
            .ToArray();
    }

    public List<Tile> GetOccupiedTiles(LayerMask layerMask)
    {
        return _tiles
            .Where(t => t.Value.Blocked)
            .Where(t => layerMask.LayerInMask(t.Value.Occupier.layer))
            .Select(t => t.Value)
            .ToList();
    }

    public List<Tile> GetFreeBottomRowTiles()
    {
        return _tiles
            .Where(t => t.Key.y == 0)
            .Where(t => !t.Value.Blocked)
            .Select(t => t.Value)
            .ToList();
    }

    public List<Tile> GetFreeTopRowTiles()
    {
        return _tiles
            .Where(t => t.Key.y == maxY)
            .Where(t => !t.Value.Blocked)
            .Select(t => t.Value)
            .ToList();
    }
}
