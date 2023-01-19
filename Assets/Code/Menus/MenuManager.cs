using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = false;

    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        if (gamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        } else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MockupLevel", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        } else 
        {
            Application.Quit();
        }
    }

    public void Options()
    {
        // open options menu
    }
}
