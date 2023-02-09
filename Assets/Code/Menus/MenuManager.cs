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
    
    [SerializeField] private GameObject startButtonText;
    [SerializeField] private GameObject resetButton;

    private bool optionsEnabled;

    private void Awake()
    {
        optionsEnabled = false;
        gamePaused = false;
        Time.timeScale = 1;
    }

    private void Start()
    {
        SetButtons();
    }

    private void SetButtons()
    {
        if (startButtonText != null)
        {
            switch (PlayerPrefs.GetInt("levelProgress"))
            {
                case 0:
                startButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "New game";
                break;

                case 1:
                startButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Continue";
                break;

                case 2:
                startButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "New game +";
                break;

                case 3:
                startButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Continue +";
                break;
            }
        }

        if (SceneManager.GetActiveScene().name == "MainMenu" && resetButton != null)
        {
            if (PlayerPrefs.GetInt("levelProgress") > 0)
            {
                resetButton.SetActive(true); 
            }
            else
            {
                resetButton.SetActive(false);
            }
        }
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
        switch (PlayerPrefs.GetInt("levelProgress"))
        {
            case 0:
            SceneManager.LoadSceneAsync("MockupLevel", LoadSceneMode.Single);
            break;

            case 1:
            SceneManager.LoadSceneAsync("BasementLevel", LoadSceneMode.Single);
            break;

            case 2:
            SceneManager.LoadSceneAsync("MockupLevel", LoadSceneMode.Single);
            break;

            case 3:
            SceneManager.LoadSceneAsync("BasementLevel", LoadSceneMode.Single);
            break;
        }
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
        SetButtons();
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

    public void ClearProgress()
    {
        PlayerPrefs.SetInt("levelProgress", 0);
    }
}
