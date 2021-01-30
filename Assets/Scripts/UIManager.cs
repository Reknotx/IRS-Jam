using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonPattern<UIManager>
{
    private int _score;
    [SerializeField] private float _secondsLeft = 600;

    [HideInInspector] public int maxScore;

    
    [SerializeField] private int minScoreRange = 3;
    
    [Header("Max score range MUST be larger than min score range.")]
    [SerializeField] private int maxScoreRange = 10;

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

    protected override void Awake()
    {
        base.Awake();
        maxScore = Random.Range(minScoreRange, maxScoreRange);
    }

    private void Update()
    {
        SecondsLeft -= Time.deltaTime;
    }

    public void AddScore()
    {
        Score++;
    }

}
