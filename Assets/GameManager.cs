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

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
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

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Destroy(pauseMenu);
    }

    public bool CheckIfPaused()
    {
        return isPaused;
    }
}