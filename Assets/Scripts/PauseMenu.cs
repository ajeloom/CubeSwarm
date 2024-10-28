using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : NetworkBehaviour
{
    private GameManager gm;
    private PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        Button resumeButton = transform.Find("Resume").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();

        player = transform.parent.gameObject.GetComponent<PlayerControls>();
    }

    private void ResumeButtonPressed()
    {
        Destroy(gameObject);
        player.ChangePause(false);
        if (!gm.isMultiplayer) {
            Time.timeScale = 1;
        }
    }

    private void QuitButtonPressed()
    {
        gm.ReturnToMainMenu();
    }
}
