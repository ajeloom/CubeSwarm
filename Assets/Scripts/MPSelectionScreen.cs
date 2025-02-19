using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MPSelectionScreen : NetworkBehaviour
{
    private Image stageImage;

    [SerializeField] private GameObject[] stages = new GameObject[2];
    [SerializeField] private Sprite[] stagesUI = new Sprite[2];

    public NetworkVariable<int> i = new NetworkVariable<int>(0);

    [SerializeField] private GameObject leavePrefab;

    private GameObject exitMenu;

    private Dictionary<ulong, bool> playerReadyDictionary;

    private bool ready = false;

    private void Awake()
    {
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        stageImage = transform.Find("Stage").gameObject.GetComponent<Image>();
        stageImage.sprite = stagesUI[0];

        Button readyButton = transform.Find("Ready").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        Button rightArrow = transform.Find("Right").gameObject.GetComponent<Button>();
        Button leftArrow = transform.Find("Left").gameObject.GetComponent<Button>();

        readyButton.onClick.AddListener(ReadyButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);
        rightArrow.onClick.AddListener(ArrowPressed);
        leftArrow.onClick.AddListener(ArrowPressed);

        rightArrow.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        leftArrow.gameObject.SetActive(NetworkManager.Singleton.IsServer);
    }

    void Update()
    {
        if (!NetworkManager.Singleton.IsServer) {
            stageImage.sprite = stagesUI[i.Value];
            return;
        }

        if (exitMenu == null) {
            SetButtonInteraction(true);
        }
        else {
            SetButtonInteraction(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        
        bool allPlayersReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                allPlayersReady = false;
                break;
            }
        }

        if (allPlayersReady) {
            StartGame();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerUnreadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = false;
    }

    private void ReadyButtonPressed()
    {
        Button readyButton = transform.Find("Ready").gameObject.GetComponent<Button>();
        TextMeshProUGUI buttonText = readyButton.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        if (!ready) {
            ready = true;
            buttonText.text = "Unready";
            SetPlayerReadyServerRpc();
        }
        else {
            ready = false;
            buttonText.text = "Ready";
            SetPlayerUnreadyServerRpc();
        }
        
    }

    private void StartGame()
    {
        GameObject instance = Instantiate(stages[i.Value]);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void BackButtonPressed()
    {
        exitMenu = Instantiate(leavePrefab);
    }
    
    private void ArrowPressed()
    {
        if (!NetworkManager.Singleton.IsServer) {
            return;
        }
 
        if (i.Value == 0) {
            stageImage.sprite = stagesUI[i.Value + 1];
            i.Value += 1;
        }
        else {
            stageImage.sprite = stagesUI[i.Value - 1];
            i.Value -= 1;
        }
    }

    private void SetButtonInteraction(bool value)
    {
        Button playButton = transform.Find("Ready").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        Button rightArrow = transform.Find("Right").gameObject.GetComponent<Button>();
        Button leftArrow = transform.Find("Left").gameObject.GetComponent<Button>();

        playButton.interactable = value;
        backButton.interactable = value;
        rightArrow.interactable = value;
        leftArrow.interactable = value;
    }
}
