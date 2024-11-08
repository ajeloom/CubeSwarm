using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    TextMeshProUGUI fpsText;
    private float time;
    private int frameCount;

    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        // QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;

        canvas = transform.Find("Canvas").gameObject;
        GameObject text = canvas.transform.Find("FPS").gameObject;
        fpsText = text.GetComponent<TextMeshProUGUI>();

        int playerFPS = PlayerPrefs.GetInt("FPS");
        ShowFPS(playerFPS == 1 ? true : false);
    }

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        int frameRate = Mathf.RoundToInt(frameCount / time);
        fpsText.text = frameRate.ToString() + " FPS";
    }

    public void ShowFPS(bool value)
    {
        canvas.SetActive(value);
    }
}
