using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int totalEnemies = 0;
    public LevelDefinition levelDefinition;

    public static string NEXT_LEVEL = "Next Level";
    public static string SCREENS_SINCE_SHOP = "Screens since shop";
    public GameEvent LevelLoaded;
    public GameEvent LevelCompleted;
    public int maxScreensUntilShop;
    public int oddsOfShop = 3;
    public GameObject advanceButton;

    private List<GameObject> _enemies = new List<GameObject>();
    private bool _levelCleared = false;

    private string _sceneToLoad = "Scenes/Battlefield";

    public Text enemyCountText;

    void Start()
    {
        LevelDefinition requestedLevel = GlobalVariables.Get<LevelDefinition>(NEXT_LEVEL);
        if (requestedLevel != null)
        {
            levelDefinition = requestedLevel;
        }
        Debug.Log("Loaded level " + levelDefinition.name);
        LevelLoaded.TriggerEvent(gameObject);

        UpdateEnemyCountText();
    }

    void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Remaining: " + totalEnemies.ToString();
        }
    }

    public void LoadNextScene()
    {
        LevelCompleted.TriggerEvent(gameObject);
        GlobalVariables.Set(NEXT_LEVEL, levelDefinition.nextLevel);
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void OnEnemySpawn(GameObject eventSource)
    {
        _enemies.Add(eventSource);
        totalEnemies++;
        UpdateEnemyCountText();
    }

    public void OnEnemyDeath(GameObject eventSource)
    {
        int removed = _enemies.RemoveAll(enemy => ReferenceEquals(enemy, eventSource));
        totalEnemies -= removed;
        UpdateEnemyCountText();
        Metrics.RegisterEnemyKilled(eventSource);
        if (!_levelCleared && _enemies.Count <= 0)
        {
            Metrics.RegisterLevelCleared(totalEnemies);
            _levelCleared = true;
            Debug.Log("Level Cleared");
            SetUpNextScene();
        }
    }

    public void OnPlayerDeath(GameObject player)
    {
        Metrics.RegisterGameOver();
        SceneManager.LoadScene("Scenes/Menus/GameOver");
    }

    public void SetUpNextScene()
    {
        // If player has filled out their cannon list already, make sure we skip the shop
        Cannon[] currentCannons = GlobalVariables.Get<Cannon[]>(ShipConfiguration.PLAYER_CANNON_DEF);
        bool playerIsMaxCannons = true;
        for (int i = 0; i < currentCannons.Length; i++)
        {
            playerIsMaxCannons = playerIsMaxCannons && currentCannons[i] != null;
        }

        int screensSinceShop = GlobalVariables.Get<int>(SCREENS_SINCE_SHOP) + 1; // default if not present is 0, add 1 so that comparison below works as expected

        int getShop = Random.Range(1, oddsOfShop + 1); // Range end is exclusive
        if (!playerIsMaxCannons && screensSinceShop >= maxScreensUntilShop || getShop == oddsOfShop)
        {
            GlobalVariables.Set(SCREENS_SINCE_SHOP, 0);
            _sceneToLoad = "Scenes/Menus/Shop";
            advanceButton.GetComponentInChildren<Text>().text = "Upgrade!";
        }
        else
        {
            GlobalVariables.Set(SCREENS_SINCE_SHOP, screensSinceShop++);
            _sceneToLoad ="Scenes/Battlefield";
            advanceButton.GetComponentInChildren<Text>().text = "Advance!";
        }
        advanceButton.SetActive(true);
    }
}
