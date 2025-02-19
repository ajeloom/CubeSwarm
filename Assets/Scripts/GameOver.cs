using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI scoreText;
    private bool newHighScore = false;

    // Start is called before the first frame update
    void Start()
    {
        Button retryButton = transform.Find("Retry").gameObject.GetComponent<Button>();
        Button mainMenuButton = transform.Find("Main Menu").gameObject.GetComponent<Button>();

        retryButton.onClick.AddListener(RetryButtonPressed);
        mainMenuButton.onClick.AddListener(MainMenuButtonPressed);

        GameManager.instance.GameOverState();

        int highScore = PlayerPrefs.GetInt("HighScore");
        if (GameManager.instance.score.Value > highScore) {
            newHighScore = true;
            highScore = GameManager.instance.score.Value;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        highScoreText = GetScoreText("High Score", highScore);
        scoreText = GetScoreText("Score", GameManager.instance.score.Value);
    }

    void Update()
    {
        if (newHighScore) {
            highScoreText.color = LerpYellow();
            scoreText.color = LerpYellow();
        }
    }

    private void RetryButtonPressed()
    {
        GameManager.instance.ResetScore();
        SceneManager.LoadScene("Stage");
    }

    private void MainMenuButtonPressed()
    {
        GameManager.instance.QuitGame();
    }

    private TextMeshProUGUI GetScoreText(string childName, int score)
    {
        GameObject obj = transform.Find(childName).gameObject;
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        text.SetText(childName + ": " + score.ToString());
        return text;
    }

    private Color LerpYellow()
    {
        return Color.Lerp(Color.white, Color.yellow, Mathf.Sin(Time.time * 5));
    }
}
