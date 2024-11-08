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
        if (fpsButton.isOn) {
            FPSLimiter fps = GetFPSLimiter();
            fps.ShowFPS(true);
            PlayerPrefs.SetInt("FPS", 1);
        }
        else {
            FPSLimiter fps = GetFPSLimiter();
            fps.ShowFPS(false);
            PlayerPrefs.SetInt("FPS", 0);
        }
    }

    private FPSLimiter GetFPSLimiter()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        FPSLimiter temp = obj.GetComponent<FPSLimiter>();
        return temp;
    }
    

    private void BackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
