using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = false;

    public void PauseGame()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            Time.timeScale = 0;
        } else 
        {
            Time.timeScale = 1;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MockupLevel", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
