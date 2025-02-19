using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MPStats : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Button retryButton = transform.Find("Retry").gameObject.GetComponent<Button>();
        Button mainMenuButton = transform.Find("Lobby").gameObject.GetComponent<Button>();

        // retryButton.onClick.AddListener(RetryButtonPressed);
        mainMenuButton.onClick.AddListener(LobbyButtonPressed);

        SetScoreText("Score", GameManager.instance.score.Value);
    }

    // private void RetryButtonPressed()
    // {
    //     GameManager.instance.ResetScore();
    //     SceneManager.LoadScene("Stage");
    // }

    private void LobbyButtonPressed()
    {
        GameManager.instance.QuitGame();
    }

    private void SetScoreText(string childName, int score)
    {
        TextMeshProUGUI text = transform.Find(childName).gameObject.GetComponent<TextMeshProUGUI>();
        text.SetText(childName + ": " + score.ToString());
    }
}
