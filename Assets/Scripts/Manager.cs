using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviourSingleton<Manager> {
    //public static Manager _instance;

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

    void Start()
    {
        //_instance = this;
    }

    void Awake()
    {
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
            GenWorld._instance.isPaused = false;
            pauseBorder.gameObject.SetActive(false);
            remotePause = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !remotePause)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                GenWorld._instance.isPaused = true;
                pauseBorder.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                GenWorld._instance.isPaused = false;
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
        GenWorld._instance.isPaused = true;
        isPaused = true;
        remotePause = true;
        pauseBorder.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        GenWorld._instance.isPaused = false;
        isPaused = false;
        remotePause = false;
        pauseBorder.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        Pause();
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
