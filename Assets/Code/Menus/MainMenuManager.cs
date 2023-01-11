using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MockupLevel", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
