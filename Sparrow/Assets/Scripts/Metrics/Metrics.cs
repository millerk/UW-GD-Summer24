using mixpanel;
using UnityEngine;

public static class Metrics
{
    public static string StartGame = "Start Game";
    public static string QuitGame = "Quit Game";
    public static string InPauseMenu = "Pause Menu";
    public static string InTitleMenu = "Title Menu";

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
        Debug.Log("Qutting game from " + quitSource);
        Value props = new Value();
        props["Quit Source"] = quitSource;
        Mixpanel.Track(QuitGame, props);
        
        // Make sure we send all remaining events
        Mixpanel.Flush();
        gameStarted = false;
    }
}