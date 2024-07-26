using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuLogic : MonoBehaviour
{
    public string newGameScene;

    public void Start()
    {
        // Metrics keeps track if this has already been called so that
        // exiting to main menu doesn't reset the total game timer
        Metrics.RegisterGameStart();
    }

    public void loadNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        Metrics.RegisterGameQuit(Metrics.InTitleMenu);
        Application.Quit();
    }
}
