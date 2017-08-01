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
        GenWorld._instance.closeMenu();
        GenWorld._instance.buildingPanel.SetActive(false);
        gameOverObject.SetActive(true);
    }
	
	public void RestartScene()
    {
        Building.buildings.Clear();
        GenWorld._instance.buildingPanel.SetActive(false);
        GenWorld._instance.closeMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
