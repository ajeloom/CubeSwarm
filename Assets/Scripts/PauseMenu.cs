using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : NetworkBehaviour
{
    private PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        Button resumeButton = transform.Find("Resume").gameObject.GetComponent<Button>();
        Button quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();

        resumeButton.onClick.AddListener(ResumeButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        player = transform.parent.gameObject.GetComponent<PlayerControls>();
    }

    private void ResumeButtonPressed()
    {
        Destroy(gameObject);
        player.ChangePause(false);
    }

    private void QuitButtonPressed()
    {
        GameManager.instance.ReturnToMainMenu();
    }
}
