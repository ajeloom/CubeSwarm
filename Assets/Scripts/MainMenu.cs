using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Awake is called even if script is not enabled
    void Awake()
    {
        Button playButton = transform.Find("Singleplayer").gameObject.GetComponent<Button>();
        Button multiplayerButton = transform.Find("Multiplayer").gameObject.GetComponent<Button>();
        Button settingsButton = transform.Find("Settings").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        playButton.onClick.AddListener(PlayButtonPressed);
        multiplayerButton.onClick.AddListener(MultiplayerButtonPressed);
        settingsButton.onClick.AddListener(SettingsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        // Destroy GameManager and NetworkManager
        if (GameManager.instance != null) {
            Destroy(GameManager.instance.gameObject);
        }

        if (NetworkManager.Singleton != null) {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    private void PlayButtonPressed()
    {
        SceneManager.LoadScene("SelectionScreen");
    }

    private void MultiplayerButtonPressed()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void SettingsButtonPressed()
    {
        SceneManager.LoadScene("Settings");
    }

    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}
