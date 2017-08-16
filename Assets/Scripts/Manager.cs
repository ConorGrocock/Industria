using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviourSingleton<Manager>
{
    [Header("Main Menu")]
    public bool isMainMenu;

    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public Button yesButton;
    public Button noButton;

    [Header("Pause")]
    public Image pauseBorder;
    public bool isPaused;
    private bool remotePause;

    [Header("Game Over")]
    public GameObject gameOverObject;
    public Button retryButton;
    public bool isGameOver;

    void Awake()
    {
        isMainMenu = SceneManager.GetActiveScene().name == "_Menu";

        if (isMainMenu) return;

        retryButton.onClick.AddListener(RestartScene);
        yesButton.onClick.AddListener(ShowTutorial);
        noButton.onClick.AddListener(NoTutorial);

        if (GenWorld._instance == null)
        {
            Debug.LogError("GenWorld instance is null");
        }

        if (isPaused)
        {
            Pause();
        }
        else
        {
            isPaused = false;
            pauseBorder.gameObject.SetActive(false);
            remotePause = false;
        }
    }

    void Update()
    {
        if (isMainMenu) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !remotePause)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                isPaused = true;
                pauseBorder.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                isPaused = false;
                pauseBorder.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    private void ShowTutorial()
    {
        tutorialPanel.SetActive(false);
        yesButton.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    private void NoTutorial()
    {
        tutorialPanel.SetActive(false);
        Unpause();
    }

    public void Pause()
    {
        isPaused = true;
        remotePause = true;
        pauseBorder.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        isPaused = false;
        remotePause = false;
        pauseBorder.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        Pause();
        GenWorld._instance.closeMenu();
        BuildingManager._instance.buildingPanel.SetActive(false);
        gameOverObject.SetActive(true);
    }

    public void RestartScene()
    {
        Building.buildings.Clear();
        BuildingManager._instance.buildingPanel.SetActive(false);
        GenWorld._instance.closeMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
