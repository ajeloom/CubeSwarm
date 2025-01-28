using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button playButton = transform.Find("Play").gameObject.GetComponent<Button>();
        Button multiplayerButton = transform.Find("Multiplayer").gameObject.GetComponent<Button>();
        Button settingsButton = transform.Find("Settings").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        playButton.onClick.AddListener(PlayButtonPressed);
        multiplayerButton.onClick.AddListener(MultiplayerButtonPressed);
        settingsButton.onClick.AddListener(SettingsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

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
