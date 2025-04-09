using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //just to set some objects
    // each enemy stats will be randomized on spawn
    private class EnemyStats
    {
        public GameObject _enemyPrefab;
        public int hp;
        public int atk;
    }

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
    private GridManager _gridManager;
    private Dictionary<Vector2, Tile> _tiles;
    private GameManager _gameManager;
    IEnumerator Start()
    {
        // as i cant set this on start i should wait till the gridmanager is set 
        // i can create a manager thatll define everythinh on start in an order but later on
        while (_gridManager == null)
        {
            _gridManager = FindObjectOfType<GridManager>();
            yield return null;
        }
        while (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
            yield return null;
        }
        Debug.Log("es");
        _tiles = _gridManager.GetTiles();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
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

    void SpawnEnemies()
    {
        var bottomTiles = GetBottomRowTiles();
        if (enemiesPerTurns.Length == 0) return;
        var turn = _gameManager.currentTurn;
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
                var tile = bottomTiles[tileIndex];
                tileIndex++;

                // Randomize stats
                int hp = Random.Range(1, enemyItem._unitMaxHP + 1);
                int atk = Random.Range(1, enemyItem._unitMaxATK + 1);

                // Instantiate enemy
                var enemyGO = Instantiate(enemyItem._unitObject, tile.transform.position, Quaternion.identity);
                enemyGO.name = $"Enemy {enemyGO.name} ({hp}HP / {atk}ATK)";
                Debug.Log(enemyGO.name);
            }
        }
    }
}
