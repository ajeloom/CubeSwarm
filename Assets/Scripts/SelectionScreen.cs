using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScreen : NetworkBehaviour
{
    private Image stageImage;

    [SerializeField] private GameObject[] stages = new GameObject[2];
    [SerializeField] private Sprite[] stagesUI = new Sprite[2];

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.StartHost();

        stageImage = transform.Find("Stage").gameObject.GetComponent<Image>();
        stageImage.sprite = stagesUI[0];

        Button playButton = transform.Find("Play").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        Button rightArrow = transform.Find("Right").gameObject.GetComponent<Button>();
        Button leftArrow = transform.Find("Left").gameObject.GetComponent<Button>();

        playButton.onClick.AddListener(PlayButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);
        rightArrow.onClick.AddListener(ArrowPressed);
        leftArrow.onClick.AddListener(ArrowPressed);
    }

    private void PlayButtonPressed()
    {
        var instance = Instantiate(stages[i]);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();

        GameManager.instance.CountDownState();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void ArrowPressed()
    {
        if (i == 0) {
            stageImage.sprite = stagesUI[i + 1];
            i += 1;
        }
        else {
            stageImage.sprite = stagesUI[i - 1];
            i -= 1;
        }
    }
}
