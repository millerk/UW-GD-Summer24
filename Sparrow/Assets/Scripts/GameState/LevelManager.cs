using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int totalEnemies = 0;
    
    private List<GameObject> _enemies = new List<GameObject>();
    private bool _levelCleared = false;

    void Update()
    {
        if (!_levelCleared && _enemies.Count <= 0)
        {
            Metrics.RegisterLevelCleared(totalEnemies);
            _levelCleared = true;
            Debug.Log("Level Cleared");
        }
    }

    public void OnEnemySpawn(GameObject eventSource)
    {
        _enemies.Add(eventSource);
        totalEnemies++;
    }

    public void OnEnemyDeath(GameObject eventSource)
    {
        _enemies.RemoveAll(enemy => ReferenceEquals(enemy, eventSource));
        Metrics.RegisterEnemyKilled(eventSource);
    }
}
