using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //to know what will be the max values for each enemy spawned

    [System.Serializable]
    public class EnemyMaxStats
    {
        [Range(0, 10)]
        public int _unitMaxCount;
        [Range(0, 100)]
        public int _unitMaxATK;
        [Range(0, 100)]
        public int _unitMaxHP;
        public GameObject _unitObject;
    }

    //to know how should i spawn each enemy per turn
    [System.Serializable]
    public class EnemiesPerTurn
    {
        public EnemyMaxStats[] Enemy;
        public int turn;
    }


    //inspector defined variables
    public EnemiesPerTurn[] enemiesPerTurns;

    //variables im going to set here
    [SerializeField] private GridManager _gridManager;
    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {

    }



    List<Tile> GetBottomRowTiles()
    {
        return _tiles
                .Where(x => x.Key.y == 0)
                .OrderBy(x => x.Key.x)
                .Select(x => x.Value)
                .ToList();
    }

    public void SpawnEnemiesForTurn(int turn)
    {
        if (_tiles == null || _tiles.Count == 0)
        {
            _tiles = _gridManager.GetTiles();
        }

        if (enemiesPerTurns.Length <= turn)
        {
            Debug.LogWarning("No enemy data for this turn.");
            return;
        }
        Debug.Log("Tile count" + _tiles.Count());
        var bottomTiles = GetBottomRowTiles();
        var enemyGroup = enemiesPerTurns[turn];
        var enemyTypes = enemyGroup.Enemy;

        int tileIndex = 0;

        foreach (var enemyItem in enemyTypes)
        {
            for (int i = 0; i < enemyItem._unitMaxCount; i++)
            {
                if (tileIndex >= bottomTiles.Count)
                {
                    Debug.LogWarning("Not enough bottom tiles to spawn all enemies.");
                    return;
                }

                var tile = bottomTiles[tileIndex++];
                int hp = Random.Range(1, enemyItem._unitMaxHP + 1);
                int atk = Random.Range(1, enemyItem._unitMaxATK + 1);

                var enemyGO = Instantiate(enemyItem._unitObject, tile.transform.position, Quaternion.identity);
                enemyGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                enemyGO.name = $"Enemy ({hp}HP / {atk}ATK)";
                var stats = enemyGO.GetComponent<EnemyStats>();
                if (stats != null)
                {
                    stats.Init(hp, atk);
                }
                else
                {
                    Debug.LogWarning("EnemyStats component missing on prefab!");
                }
            }
        }
    }
}
