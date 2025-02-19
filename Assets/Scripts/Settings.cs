using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    private Toggle fpsButton;
    
    void Start()
    {
        fpsButton = transform.Find("Toggle").gameObject.GetComponent<Toggle>();
        Button backButton = transform.Find("Back").gameObject.GetComponent<Button>();

        backButton.onClick.AddListener(BackButtonPressed);

        int playerFPS = PlayerPrefs.GetInt("FPS");
        fpsButton.isOn = playerFPS == 1 ? true : false;
    }

    void Update()
    {
        FPSLimiter fps = GameManager.instance.gameObject.GetComponent<FPSLimiter>();
        if (fpsButton.isOn) {
            fps.ShowFPS(true);
            PlayerPrefs.SetInt("FPS", 1);
        }
        else {
            fps.ShowFPS(false);
            PlayerPrefs.SetInt("FPS", 0);
        }
    }

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
