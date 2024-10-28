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

    public NetworkVariable<int> i = new NetworkVariable<int>(0);

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        networkManager.StartHost();

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
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.SetStage(stages[i.Value]);
        NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void ArrowPressed()
    {
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
