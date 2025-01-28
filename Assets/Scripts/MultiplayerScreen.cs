using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MultiplayerScreen : NetworkBehaviour
{
    void Start()
    {
        Button hostButton = transform.Find("Host").gameObject.GetComponent<Button>();
        Button joinButton = transform.Find("Join").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        hostButton.onClick.AddListener(HostButtonPressed);
        joinButton.onClick.AddListener(JoinButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);
    }

    private void HostButtonPressed()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerSelection", LoadSceneMode.Single);
    }

    private void JoinButtonPressed()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
