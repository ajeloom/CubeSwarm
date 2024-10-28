using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MPStats : NetworkBehaviour
{
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Button retryButton = transform.Find("Retry").gameObject.GetComponent<Button>();
        Button mainMenuButton = transform.Find("Lobby").gameObject.GetComponent<Button>();

        // retryButton.onClick.AddListener(RetryButtonPressed);
        mainMenuButton.onClick.AddListener(LobbyButtonPressed);

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();

        SetScoreText("Score", gm.score.Value);
    }

    // private void RetryButtonPressed()
    // {
    //     gm.ResetScore();
    //     SceneManager.LoadScene("Stage");
    // }

    private void LobbyButtonPressed()
    {
        gm.ReturnToMainMenu();
    }

    private void SetScoreText(string childName, int score)
    {
        TextMeshProUGUI text = transform.Find(childName).gameObject.GetComponent<TextMeshProUGUI>();
        text.SetText(childName + ": " + score.ToString());
    }
}
