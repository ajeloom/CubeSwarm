using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private GameManager gm;
    private TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        GameObject canvas = transform.GetChild(0).gameObject;
        GameObject scoreText = canvas.transform.GetChild(5).gameObject;
        score = scoreText.GetComponent<TextMeshProUGUI>();

        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        gm = obj.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        score.SetText("Score: " + gm.score.ToString());
    }
}
