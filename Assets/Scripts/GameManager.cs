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

    public const int MAX_PLAYERS = 4;

    // Events
    public event EventHandler OnGameStart;
    public event EventHandler OnTryingToJoin;

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
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private List<Vector3> spawnPositions;

    public NetworkVariable<int> score = new NetworkVariable<int>(0);

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

    void Start()
    {
        if (NetworkManager.Singleton != null) {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        }
    }

    /// <summary>
    /// Start as host and subscribes to events
    /// </summary>
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }

    /// <summary>
    /// Start as client and subscribes to events
    /// </summary>
    public void StartClient()
    {
        OnTryingToJoin?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.StartClient();
    }

    /// <summary>
    /// Handles if the client can join a game
    /// </summary>
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (SceneManager.GetActiveScene().name != "MultiplayerSelection") {
            response.Approved = false;
            response.Reason = "Game already started";
            return;
        }
        else if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERS) {
            response.Approved = false;
            response.Reason = "Game is full";
            return;
        }

        response.Approved = true;
        // response.Position = new Vector3(20.0f, 0.0f, 0.0f);
        // response.Position = spawnPositions[0];
        // response.CreatePlayerObject = true;
    }

    /// <summary>
    /// Spawns the player prefabs on a spawn point
    /// </summary>
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (sceneName == "Game" && NetworkManager.Singleton.IsServer) {
            int i = 0;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
                Transform playerTransform = Instantiate(playerPrefab, spawnPositions[i], Quaternion.identity);
                playerTransform.position = spawnPositions[i];
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
                i++;
            }
            CountDownState();
        }
    }

    void Update()
    {
        switch (state.Value) {
            case State.CountDown:
                // Start the countdown
                countDownText.gameObject.SetActive(true);
                countDownTimer -= Time.deltaTime;
                int time = (int)countDownTimer;

                if (time > 0) {
                    countDownText.text = time.ToString();
                }
                else {
                    if (NetworkManager.Singleton.IsServer) {
                        state.Value = State.GameStart;
                    }
                }
                break;
            case State.GameStart:
                countDownText.gameObject.SetActive(false);
                if (!NetworkManager.Singleton.IsServer) {
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

    /// <summary>
    /// Tells all clients that the game has started
    /// </summary>
    [ClientRpc]
    private void ReadyClientRpc()
    {
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Resets the score to zero for each new game
    /// </summary>
    public void ResetScore()
    {
        score.Value = 0;
    }

    [ClientRpc]
    public void ReturnToMainMenuClientRpc()
    {
        ReturnToMainMenu();
    }

    public void QuitGame()
    {
        if (NetworkManager.Singleton.IsServer) {
            ReturnToMainMenuClientRpc();
        }
        else {
            ReturnToMainMenu();
        }
    }

    /// <summary>
    /// Will return the player back to the main menu
    /// Called when player exits during gameplay
    /// </summary>
    public void ReturnToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu");
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