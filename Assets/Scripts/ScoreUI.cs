using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI score;

    private int localScore = 0;
    private bool incrementingScore = false;

    private TextMeshProUGUI wave;

    private void Awake()
    {
        GameManager.instance.OnWaveStart += NewWaveStarted;
    }

    private void NewWaveStarted(object sender, EventArgs e)
    {
        wave.SetText("Wave " + GameManager.instance.waveNumber.Value.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.Find("Canvas").gameObject;
        GameObject scoreText = canvas.transform.Find("Score").gameObject;
        score = scoreText.GetComponent<TextMeshProUGUI>();

        score.SetText("Score: " + GameManager.instance.score.Value.ToString());

        GameObject waveText = canvas.transform.Find("Wave").gameObject;
        wave = waveText.GetComponent<TextMeshProUGUI>();

        wave.SetText("Wave " + GameManager.instance.waveNumber.Value.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (localScore < GameManager.instance.score.Value && !incrementingScore) {
            incrementingScore = true;
            localScore++;
            score.SetText("Score: " + localScore.ToString());
            incrementingScore = false;
        }
    }
}
