using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuLogic : MonoBehaviour
{
    public string newGameScene;
    public void loadNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
