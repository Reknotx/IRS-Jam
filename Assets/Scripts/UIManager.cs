using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int _score;
    private float _secondsLeft;

    public int maxScore;

    
    public int minScoreRange;
    
    [Header("Max score range MUST be larger than min score range.")]
    public int maxScoreRange;

    public float SecondsLeft
    {
        get => _secondsLeft;
        set
        {
            _secondsLeft = value;
            
            if (timeText != null)
            {
                float minutes = _secondsLeft / 60;
                float seconds = _secondsLeft % 60;

                if (seconds < 10)
                    timeText.text = "Time remaining: " + minutes + ":0" + seconds;
                else
                    timeText.text = "Time remaining: " + minutes + ":" + seconds;
            }
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            _score += value;
            
            if (scoreText != null)
            {
                scoreText.text = "Score: " + _score + "/" + maxScore;
            }
        }
    }

    public Text scoreText, timeText;

    private void Awake()
    {
        maxScore = Random.Range(minScoreRange, maxScoreRange);
    }

    private void Update()
    {
        SecondsLeft -= Time.deltaTime;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

}
