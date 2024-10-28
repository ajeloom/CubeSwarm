using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MultiplayerScreen : NetworkBehaviour
{
    private NetworkManager m_NetworkManager;
    // [SerializeField] private GameObject leavePrefab;

    void Start()
    {
        m_NetworkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();

        Button hostButton = transform.Find("Host").gameObject.GetComponent<Button>();
        Button joinButton = transform.Find("Join").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        hostButton.onClick.AddListener(HostButtonPressed);
        joinButton.onClick.AddListener(JoinButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);
    }

    private void HostButtonPressed()
    {
        m_NetworkManager.StartHost();
        NetworkManager.SceneManager.LoadScene("MultiplayerSelection", LoadSceneMode.Single);
    }

    private void JoinButtonPressed()
    {
        m_NetworkManager.StartClient();
    }

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
