using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<EnemyController> _enemies = new List<EnemyController>();
    private bool _isProcessing = false;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MoveAllEnemies()
    {
        if (!_isProcessing)
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
        _isProcessing = true;

        foreach (var enemy in _enemies)
        {
            yield return enemy.ContinueMovementCoroutine(); // 👈 wait until this enemy finishes
            yield return new WaitForSeconds(0.2f); // optional delay between each
        }

        _isProcessing = false;
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }

}
