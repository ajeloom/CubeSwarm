using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        Button resumeButton = transform.Find("Resume").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();
    }

    private void ResumeButtonPressed()
    {
        gm.ResumeGame();
    }

    private void QuitButtonPressed()
    {
        Time.timeScale = 1;
        gm.ReturnToMainMenu();
    }
}
