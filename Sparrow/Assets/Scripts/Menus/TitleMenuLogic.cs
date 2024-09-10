using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuLogic : MonoBehaviour
{
    public DialogueLevelDefinition introDialogue;

    public void Start()
    {
        // Metrics keeps track if this has already been called so that
        // exiting to main menu doesn't reset the total game timer
        Metrics.RegisterGameStart();
    }

    public void loadNewGame()
    {
        GlobalVariables.ClearAll();
        // First scene is a dialogue scene, set the next enemy level to whatever it is pointing to
        GlobalVariables.Set(LevelManager.NEXT_LEVEL, introDialogue.nextLevel);
        SceneManager.LoadScene(introDialogue.DialogueScene);
    }

    public void QuitGame()
    {
        Metrics.RegisterGameQuit(Metrics.InTitleMenu);
        Application.Quit();
    }
    public void settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
