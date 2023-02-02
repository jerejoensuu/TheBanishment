using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject mainMenu;

    private bool optionsEnabled;

    private void Awake()
    {
        optionsEnabled = false;
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (pauseMenu != null && !optionsEnabled)
        {
            gamePaused = !gamePaused;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        if (optionsEnabled)
        {
            ToggleOptions();
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

    public void ToggleOptions()
    {
        optionsEnabled = !optionsEnabled;
        options.SetActive(optionsEnabled);

        if (mainMenu == null && pauseMenu == null) 
        { 
            return; 
        } else if (mainMenu != null && pauseMenu == null)
        {
            mainMenu.SetActive(!optionsEnabled);
        } else 
        {
            pauseMenu.SetActive(!optionsEnabled);
        }
    }
}
