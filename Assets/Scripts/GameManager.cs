using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;
    private GameObject player;

    public bool isMultiplayer = false;

    public NetworkVariable<int> score = new NetworkVariable<int>(0);

    private GameObject stagePrefab;
    private bool stageLoaded = false;

    public string currentScene;
    private NetworkManager networkManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene.name;
        if (scene.name == "SelectionScreen") {
            isMultiplayer = false;
            networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        }
        else if (scene.name == "MultiplayerSelection") {
            isMultiplayer = true;
            networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        }
        else if (scene.name == "Game") {
            if (IsServer) {
                if (!stageLoaded) {
                    stageLoaded = true;

                    // Spawn the stage
                    var instance = Instantiate(stagePrefab);
                    var instanceNetworkObject = instance.GetComponent<NetworkObject>();
                    instanceNetworkObject.Spawn();

                    // Spawn the enemy spawners
                    for (int i = 0; i < 4; i++) {
                        GameObject stg = GameObject.FindGameObjectWithTag("Stage");
                        GameObject obj = stg.transform.Find("EnemySpawn" + i.ToString()).gameObject;
                        var spawner = Resources.Load<GameObject>("Prefabs/Network/EnemySpawner");
                        instance = Instantiate(spawner, obj.transform.position, obj.transform.rotation);
                        instanceNetworkObject = instance.GetComponent<NetworkObject>();
                        instanceNetworkObject.Spawn();
                    }
                }

                // Keep track of every player's health
                checkPlayersHealth();
            }
        }
        else {
            stageLoaded = false;
        }
    }

    public void ResetScore()
    {
        score.Value = 0;
    }

    public void ReturnToMainMenu()
    {
        ResetScore();
        if (isMultiplayer) {
            if (networkManager.IsServer) {
                networkManager.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            else {
                networkManager.Shutdown();
                Destroy(networkManager.gameObject);
                SceneManager.LoadScene("MainMenu");
            }
        }
        else {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SetStage(GameObject obj) 
    {
        stagePrefab = obj;
    }

    public void checkPlayersHealth()
    {
        // Keep track of every player's health
        if (!checkedPlayerHealth) {
            checkedPlayerHealth = true;

            for (int i = 0; i < NetworkManager.ConnectedClients.Count; i++) {
                player = NetworkManager.ConnectedClients[(ulong)i].PlayerObject.gameObject;
                if (player.GetComponent<HealthComponent>().currentHP.Value > 0.0f) {
                    checkedPlayerHealth = false;
                    return;
                }
            }

            // Game Over when all players are dead
            networkManager.SceneManager.LoadScene("MPGameOver", LoadSceneMode.Single);
            
            checkedPlayerHealth = false;
        }
    }
}