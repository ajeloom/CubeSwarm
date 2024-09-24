using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private GameObject player;

    private bool isPaused = false;
    [SerializeField] private GameObject pausePrefab;
    private GameObject pauseMenu;

    public int score = 0;

    private GameObject stagePrefab;
    private bool stageLoaded = false;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Stage") {
            if (!stageLoaded) {
                stageLoaded = true;
                LoadStage();
            }

            player = GameObject.FindGameObjectWithTag("Player");
            HealthComponent healthComponent = player.GetComponent<HealthComponent>();
            if (healthComponent.currentHP <= 0.0f) {
                SceneManager.LoadScene("GameOver");
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
                isPaused = true;
                Time.timeScale = 0;
                pauseMenu = Instantiate(pausePrefab, Vector3.zero, transform.rotation);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
                ResumeGame();
            }
        }
        else if (scene.name == "SelectionScreen") {
            SelectionScreen screen = GameObject.FindWithTag("Selection").GetComponent<SelectionScreen>();
            stagePrefab = screen.GetStage();
        }
        else {
            stageLoaded = false;
        }
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Destroy(pauseMenu);
    }

    public void ReturnToMainMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
        ResetScore();
    }

    public bool CheckIfPaused()
    {
        return isPaused;
    }

    public void LoadStage()
    {
        Instantiate(stagePrefab, Vector3.zero, transform.rotation);
    }
}