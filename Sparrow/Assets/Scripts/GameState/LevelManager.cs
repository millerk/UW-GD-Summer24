using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int totalEnemies = 0;
    public LevelDefinition levelDefinition;

    public static string NEXT_LEVEL = "Next Level";
    public GameEvent LevelLoaded;
    
    private List<GameObject> _enemies = new List<GameObject>();
    private bool _levelCleared = false;

    void Start()
    {
        LevelDefinition requestedLevel = GlobalVariables.Get<LevelDefinition>(NEXT_LEVEL);
        if (requestedLevel != null)
        {
            levelDefinition = requestedLevel;
        }
        Debug.Log("Loaded level " + levelDefinition.name);
        LevelLoaded.TriggerEvent(gameObject);
    }

    public void LoadNextLevel()
    {
        GlobalVariables.Set(NEXT_LEVEL, levelDefinition.nextLevel);
        SceneManager.LoadScene("Scenes/Battlefield");
    }


    void Update()
    {

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
        if (!_levelCleared && _enemies.Count <= 0)
        {
            Metrics.RegisterLevelCleared(totalEnemies);
            _levelCleared = true;
            Debug.Log("Level Cleared");
            // TODO: Add timer or some mechanism to gate this so player has time to collect loot
            LoadNextLevel();
        }
    }
}
