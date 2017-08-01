using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void StartGame()
    {
        SceneManager.LoadScene("_Level_w_Lab");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        Building.buildings.Clear();
        GenWorld._instance.closeMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
