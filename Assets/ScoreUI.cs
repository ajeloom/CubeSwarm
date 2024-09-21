using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private GameManager gm;
    private TextMeshProUGUI score;

    private int localScore = 0;
    private bool incrementingScore = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject scoreText = canvas.transform.GetChild(5).gameObject;
        score = scoreText.GetComponent<TextMeshProUGUI>();

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();

        score.SetText("Score: " + gm.score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (localScore < gm.score && !incrementingScore) {
            incrementingScore = true;
            localScore++;
            score.SetText("Score: " + localScore.ToString());
            incrementingScore = false;
        }
    }
}
