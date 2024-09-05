using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenLogic : MonoBehaviour
{
    public void RestartGame()
    {
        GlobalVariables.ClearAll();
        SceneManager.LoadScene("Scenes/Menus/TitleMenu");
    }
}
