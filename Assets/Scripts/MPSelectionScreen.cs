using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MPSelectionScreen : NetworkBehaviour
{
    private Image stageImage;
    private NetworkManager m_NetworkManager;

    [SerializeField] private GameObject[] stages = new GameObject[2];
    [SerializeField] private Sprite[] stagesUI = new Sprite[2];

    public NetworkVariable<int> i = new NetworkVariable<int>(0);

    [SerializeField] private GameObject leavePrefab;

    private GameObject exitMenu;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        m_NetworkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();

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

        if (!IsHost) {
            rightArrow.gameObject.SetActive(false);
            leftArrow.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsHost) {
            stageImage.sprite = stagesUI[i.Value];
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
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.SetStage(stages[i.Value]);
        NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void BackButtonPressed()
    {
        exitMenu = Instantiate(leavePrefab);
    }
    
    private void ArrowPressed()
    {
        if (IsHost) {
            if (i.Value == 0) {
                stageImage.sprite = stagesUI[i.Value + 1];
                i.Value += 1;
            }
            else {
                stageImage.sprite = stagesUI[i.Value - 1];
                i.Value -= 1;
            }
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
