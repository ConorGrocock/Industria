using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [Header("Game Over")]
    public GameObject gameOverObject;
    public Button retryButton;

    void Awake()
    {
        retryButton.onClick.AddListener(RestartScene);
    }

    public void ShowGameOver()
    {
        gameOverObject.SetActive(true);
    }
	
	public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("_Level");
    }
}
