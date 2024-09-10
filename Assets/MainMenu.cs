using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton, quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(PlayButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }

    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}
