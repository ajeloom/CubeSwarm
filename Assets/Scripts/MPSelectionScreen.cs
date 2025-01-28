using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MPSelectionScreen : NetworkBehaviour
{
    private Image stageImage;

    [SerializeField] private GameObject[] stages = new GameObject[2];
    [SerializeField] private Sprite[] stagesUI = new Sprite[2];

    public NetworkVariable<int> i = new NetworkVariable<int>(0);

    [SerializeField] private GameObject leavePrefab;

    private GameObject exitMenu;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
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

        if (!IsServer) {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsServer) {
            stageImage.sprite = stagesUI[i.Value];
            return;
        }

        if (exitMenu == null) {
            SetButtonInteraction(true);
        }
        else {
            SetButtonInteraction(false);
        }
    }


    private void PlayButtonPressed()
    {
        if (!IsServer) {
            return;
        }

        GameObject instance = Instantiate(stages[i.Value]);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();

        // TODO: Move the players to the correct spawn point

        GameManager.instance.CountDownState();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void BackButtonPressed()
    {
        exitMenu = Instantiate(leavePrefab);
    }
    
    private void ArrowPressed()
    {
        if (!IsServer) {
            return;
        }
 
        if (i.Value == 0) {
            stageImage.sprite = stagesUI[i.Value + 1];
            i.Value += 1;
        }
        else {
            stageImage.sprite = stagesUI[i.Value - 1];
            i.Value -= 1;
        }
    }

    private void SetButtonInteraction(bool value)
    {
        Button playButton = transform.Find("Play").gameObject.GetComponent<Button>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        Button rightArrow = transform.Find("Right").gameObject.GetComponent<Button>();
        Button leftArrow = transform.Find("Left").gameObject.GetComponent<Button>();

        playButton.interactable = value;
        backButton.interactable = value;
        rightArrow.interactable = value;
        leftArrow.interactable = value;
    }
}
