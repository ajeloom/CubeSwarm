using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        Button retryButton = transform.Find("Retry").gameObject.GetComponent<Button>();
        Button mainMenuButton = transform.Find("Main Menu").gameObject.GetComponent<Button>();

        retryButton.onClick.AddListener(RetryButtonPressed);
        mainMenuButton.onClick.AddListener(MainMenuButtonPressed);

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();

        GameObject scoreText = transform.GetChild(2).gameObject;
        TextMeshProUGUI score = scoreText.GetComponent<TextMeshProUGUI>();
        score.SetText("Score: " + gm.score.ToString());
    }

    private void RetryButtonPressed()
    {
        gm.ResetScore();
        SceneManager.LoadScene("Stage1");
    }

    private void MainMenuButtonPressed()
    {
        gm.ReturnToMainMenu();
    }
}
