using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2Int, Tile> _tiles;
    void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2Int, Tile>();
        foreach (var tile in GetComponentsInChildren<Tile>())
        {
            _tiles[tile.coords] = tile;
            var x = tile.coords.x;
            var y = tile.coords.y;
        }
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

    public Dictionary<Vector2Int, Tile> GetTiles()
    {
        return _tiles;
    }
}
