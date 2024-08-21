using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuLogic : MonoBehaviour
{
    public string newGameScene;
    public LevelDefinition gameStartLevel;

    public void Start()
    {
        // Metrics keeps track if this has already been called so that
        // exiting to main menu doesn't reset the total game timer
        Metrics.RegisterGameStart();
    }

    public void loadNewGame()
    {
        GlobalVariables.Set(LevelManager.NEXT_LEVEL, gameStartLevel);
        SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        Metrics.RegisterGameQuit(Metrics.InTitleMenu);
        Application.Quit();
    }
}
