using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public ScoreController Controller { private get; set; }

    private void Start()
    {
        scoreText.text = Controller.GetCurrentScore().ToString();
    }

    public void UpdateScoreUI(int scoreToDisplay)
    {
        scoreText.text = scoreToDisplay.ToString();
    }
}
