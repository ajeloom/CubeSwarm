using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button playButton = transform.Find("Play").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        playButton.onClick.AddListener(PlayButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    private void PlayButtonPressed()
    {
        SceneManager.LoadScene("SelectionScreen");
    }

    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}
