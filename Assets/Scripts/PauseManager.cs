using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    public GameManager gameManager;

    public void Resume()
    {
        gameManager.Resume();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneName: "IntroMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
