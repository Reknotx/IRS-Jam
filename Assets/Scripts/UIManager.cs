using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int _score;
    [SerializeField] private float _secondsLeft = 600;

    [HideInInspector] public int maxScore;

    
    public int minScoreRange = 3;
    
    [Header("Max score range MUST be larger than min score range.")]
    public int maxScoreRange = 10;

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
                    timeText.text = "Time left: " + Mathf.Floor(minutes).ToString() + ":0" + Mathf.Floor(seconds).ToString();
                else
                    timeText.text = "Time left: " + Mathf.Floor(minutes).ToString() + ":" + Mathf.Floor(seconds).ToString();
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
