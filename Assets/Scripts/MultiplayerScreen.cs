using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MultiplayerScreen : NetworkBehaviour
{
    [SerializeField] private GameObject connectingUI;

    private void Awake()
    {
        GameManager.instance.OnTryingToJoin += TryingToJoinGame;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

        Button hostButton = transform.Find("Host").gameObject.GetComponent<Button>();
        Button joinButton = transform.Find("Join").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        hostButton.onClick.AddListener(HostButtonPressed);
        joinButton.onClick.AddListener(JoinButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);

        Hide(connectingUI);
    }

    /// <summary>
    /// Host a game
    /// </summary>
    private void HostButtonPressed()
    {
        GameManager.instance.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerSelection", LoadSceneMode.Single);
    }

    /// <summary>
    /// Join the game as a client
    /// </summary>
    private void JoinButtonPressed()
    {
        GameManager.instance.StartClient();
    }

    /// <summary>
    /// Return to the main menu
    /// </summary>
    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Shows UI when connecting to a game
    /// </summary>
    private void TryingToJoinGame(object obj, System.EventArgs eventArgs)
    {
        Show(connectingUI);
    }

    /// <summary>
    /// Handles when a client or host disconnects from the game
    /// </summary>
    private void OnClientDisconnectCallback(ulong clientId)
    {
        if (NetworkManager.Singleton.DisconnectReason != string.Empty) {
            Debug.Log($"Approval Declined Reason: {NetworkManager.Singleton.DisconnectReason}");
        }

        if (NetworkManager.Singleton.IsServer && clientId == OwnerClientId) {
            SceneManager.LoadScene("MainMenu");
        }
        else if (!NetworkManager.Singleton.IsServer && clientId == OwnerClientId) {
            SceneManager.LoadScene("LobbyJoinFailed");
        }
    }

    /// <summary>
    /// Show the GameObject
    /// </summary>
    private void Show(GameObject obj)
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// Hide the GameObject
    /// </summary>
    private void Hide(GameObject obj)
    {
        obj.SetActive(false);
    }

    /// <summary>
    /// Disconnect from event callbacks
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.instance.OnTryingToJoin -= TryingToJoinGame;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }
}
