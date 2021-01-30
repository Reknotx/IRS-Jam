using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonPattern<UIManager>
{
    private int _score;
    private float _secondsLeft = 600;

    [HideInInspector] public int maxScore;

    
    [SerializeField] private int minScoreRange = 3;
    
    [Header("Max score range MUST be larger than min score range.")]
    [SerializeField] private int maxScoreRange = 10;

    public GameObject gameWinCanvas, gameLostCanvas;

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

                if (_secondsLeft <= 0) DisplayGameEnd(false);
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

            if (_score == maxScore) DisplayGameEnd(true);
        }
    }

    public Text scoreText, timeText;

    protected override void Awake()
    {
        base.Awake();
        maxScore = Random.Range(minScoreRange, maxScoreRange);
        Score = 0;
    }

    private void Update()
    {
        SecondsLeft -= Time.deltaTime;
    }

    public void AddScore()
    {
        Score++;
    }

    private void DisplayGameEnd(bool gameWon)
    {
        Time.timeScale = 0f;

        if (gameWon)
            DisplayGameWin();
        else
            DisplayGameLost();
    }

    private void DisplayGameWin()
    {
        if (gameWinCanvas != null)
        {
            gameWinCanvas.SetActive(true);
        }
    }

    private void DisplayGameLost()
    {
        if (gameLostCanvas != null)
        {
            gameLostCanvas.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
