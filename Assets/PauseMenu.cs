using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button resumeButton = transform.Find("Resume").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    private void ResumeButtonPressed()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        GameManager gm = obj.GetComponent<GameManager>();
        gm.ResumeGame();
    }

    private void QuitButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
