using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    public Button helpButton;
    public Button quitButton;

    void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Start a new game
    /// </summary>
    public void StartGame()
    {
        Analytics.CustomEvent("Game started");
        Building.buildings.Clear();
        SceneManager.LoadScene("_Level");
    }

    public void QuitGame()
    {
        Analytics.CustomEvent("Game quit");
        Application.Quit();
    }

    public void RestartScene()
    {
        Analytics.CustomEvent("Game restarted");
        Building.buildings.Clear();
        GenWorld._instance.closeMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
