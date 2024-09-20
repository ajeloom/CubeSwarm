using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button retryButton = transform.Find("Retry").gameObject.GetComponent<Button>();
        Button mainMenuButton = transform.Find("Main Menu").gameObject.GetComponent<Button>();

        retryButton.onClick.AddListener(RetryButtonPressed);
        mainMenuButton.onClick.AddListener(MainMenuButtonPressed);
    }

    private void RetryButtonPressed()
    {
        SceneManager.LoadScene("Stage1");
    }

    private void MainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
