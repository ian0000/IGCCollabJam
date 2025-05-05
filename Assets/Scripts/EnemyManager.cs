using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    List<EnemyController> _enemies = new List<EnemyController>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MoveAllEnemies()
    {
        StartCoroutine(MoveEnemiesSequentially());
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    private IEnumerator MoveEnemiesSequentially()
    {
        foreach (var enemy in _enemies)
        {
            yield return enemy.MoveChunk(); // 👈 wait until this enemy finishes
            yield return new WaitForSeconds(0.2f); // optional delay between each
        }
        GameManager.Instance.ChangeState(GameState.EnemyAttack);
    }
}
