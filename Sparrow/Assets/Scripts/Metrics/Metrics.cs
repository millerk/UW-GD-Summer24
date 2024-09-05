using mixpanel;
using UnityEngine;

public static class Metrics
{
    // Game state changes
    public static string StartGame = "Start Game";
    public static string QuitGame = "Quit Game";
    public static string InPauseMenu = "Pause Menu";
    public static string InTitleMenu = "Title Menu";
    public static string LevelCompleted = "Level Completed";
    public static string GameOver = "GameOver";

    // Combat events
    public static string EnemyKilled = "Enemy Killed";

    public static bool gameStarted = false;

    public static void RegisterGameStart()
    {
        if (!gameStarted)
        {
            Debug.Log("Starting game");
            Mixpanel.Track(StartGame);
            gameStarted = true;
        } 
    }

    public static void RegisterGameQuit(string quitSource)
    {   
        Value props = new Value();
        props["Quit Source"] = quitSource;
        Mixpanel.Track(QuitGame, props);
        
        // Make sure we send all remaining events
        Mixpanel.Flush();
        gameStarted = false;
    }

    public static void RegisterEnemyKilled(GameObject enemy)
    {
        Value props = new Value();
        props["Enemy Type"] = enemy.name;
        // Some other things to put in here, probably
        Mixpanel.Track(EnemyKilled, props);
    }

    public static void RegisterLevelCleared(int totalEnemies)
    {
        Value props = new Value();
        props["Total enemies"] = totalEnemies;
        Mixpanel.Track(LevelCompleted, props);
    }

    public static void RegisterGameOver()
    {
        Mixpanel.Track(GameOver);
    }
}