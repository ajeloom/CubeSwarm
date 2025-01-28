using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { get; private set; }

    // Events
    public event EventHandler OnGameStart; 

    public enum State {
        Menu,
        CountDown,
        GameStart,
        GameOver
    };

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Menu);
    private float countDownTimer = 4.0f;

    [SerializeField] private TextMeshProUGUI countDownText;

    private GameObject player;

    public NetworkVariable<int> score = new NetworkVariable<int>(0);

    private GameObject stagePrefab;
    private bool loadedEnemies = false;

    public string currentScene;

    private bool checkedPlayerHealth = false;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        countDownText.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (state.Value) {
            case State.Menu:
                loadedEnemies = false;
                break;
            case State.CountDown:
                // Start the countdown
                countDownText.gameObject.SetActive(true);
                countDownTimer -= Time.deltaTime;
                int time = (int)countDownTimer;

                if (time > 0) {
                    countDownText.text = time.ToString();
                }
                else {
                    state.Value = State.GameStart;
                }
                break;
            case State.GameStart:
                countDownText.gameObject.SetActive(false);
                if (!IsServer) {
                    return;
                }

                if (!loadedEnemies) {
                    loadedEnemies = true;
                    ReadyClientRpc();

                    // Spawn the enemy spawners
                    for (int i = 0; i < 4; i++) {
                        GameObject stg = GameObject.FindGameObjectWithTag("Stage");
                        GameObject obj = stg.transform.Find("EnemySpawn" + i.ToString()).gameObject;
                        var spawner = Resources.Load<GameObject>("Prefabs/Network/EnemySpawner");
                        var instance = Instantiate(spawner, obj.transform.position, obj.transform.rotation);
                        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
                        instanceNetworkObject.Spawn();
                    }
                }
                
                // Keep track of every player's health
                // checkPlayersHealthServerRpc();
                break;
            case State.GameOver:
                break;
        }
    }

    [ClientRpc]
    private void ReadyClientRpc()
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void LoadStageClientRpc()
    {
        Instantiate(stagePrefab);
    }

    /// <summary>
    /// Resets the score to zero for each new game
    /// </summary>
    public void ResetScore()
    {
        score.Value = 0;
    }

    /// <summary>
    /// Will return the player back to the main menu
    /// Called when player exits during gameplay
    /// </summary>
    public void ReturnToMainMenu()
    {
        ResetScore();
        if (IsServer) {
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("MainMenu");
        }

    }

    /// <summary>
    /// Checks the health of every player
    /// Will load the game over screen if every player is dead
    /// </summary>
    public void checkPlayersHealth() {
        if (!checkedPlayerHealth) {
            checkedPlayerHealth = true;
            for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++) {
                player = NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.gameObject;
                if (player.GetComponent<HealthComponent>().currentHP.Value > 0.0f) {
                    checkedPlayerHealth = false;
                    return;
                }
            }

            // Game Over when all players are dead
            state.Value = State.GameOver;
            NetworkManager.Singleton.SceneManager.LoadScene("MPGameOver", LoadSceneMode.Single);
            
            checkedPlayerHealth = false;
        }
    }

    /// <summary>
    /// Adds the given enemy's points to the score
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(int value)
    {
        score.Value += value;
    }

    /// <summary>
    /// Returns the game state of the GameManager
    /// </summary>
    /// <returns></returns>
    public State GetState()
    {
        return state.Value;
    }

    /// <summary>
    /// Changes the game state to the Menu
    /// which will ignore any code needed during gameplay
    /// </summary>
    /// <returns></returns>
    public void MenuState()
    {
        state.Value = State.Menu;
    }

    /// <summary>
    /// Changes the game state to the Countdown
    /// which is the time before the game starts
    /// </summary>
    /// <returns></returns>
    public void CountDownState()
    {
        state.Value = State.CountDown;
    }

    /// <summary>
    /// Changes the game state to Game Start
    /// which allows gameplay to commence
    /// </summary>
    /// <returns></returns>
    public void GameStartState()
    {
        state.Value = State.GameStart;
    }

    /// <summary>
    /// Changes the game state to Game Over
    /// which stops gameplay and shows your final score
    /// </summary>
    /// <returns></returns>
    public void GameOverState()
    {
        state.Value = State.GameOver;
    }
}