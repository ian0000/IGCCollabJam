using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] int _unitMaxCount;
    [SerializeField] EnemyController _enemyPrefab;

    public void SpawnEnemiesForTurn(int turn)
    {
        var freeTiles = GridManager.Instance.GetFreeBottomRowTiles();
        for (int i = 0; i < _unitMaxCount; i++)
        {
            if (freeTiles.Count == 0) return;

            var tile = freeTiles[Random.Range(0, freeTiles.Count)];
            freeTiles.Remove(tile);
            var enemy = Instantiate(_enemyPrefab, tile.transform.position, Quaternion.identity);
            enemy.currentTile = tile;
        }

        GameManager.Instance.ChangeState(GameState.PlayCards);
    }
}
