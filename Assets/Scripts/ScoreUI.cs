using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI score;

    private int localScore = 0;
    private bool incrementingScore = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject scoreText = canvas.transform.GetChild(5).gameObject;
        score = scoreText.GetComponent<TextMeshProUGUI>();

        score.SetText("Score: " + GameManager.instance.score.Value.ToString());
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
