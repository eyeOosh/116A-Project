using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Text scoreText;  // assign in Inspector
    private float score = 0;
    private float lastScore = 0;

    private void Awake()
    {
        // Singleton pattern so any script can access the score
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    // Call this to add points
    public void AddScore(float points)
    {
        score += points;
        lastScore = points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText is null)
            return;

        if(lastScore > 0)
        {
            scoreText.text = $"{score} / +{Math.Abs(lastScore)}";
            scoreText.color = Color.green;
        }
        else if(lastScore < 0)
        {
            scoreText.text = $"{score} / -{Math.Abs(lastScore)}";
            scoreText.color = Color.red;
        }
        else
        {
            scoreText.text = $"{score}";
            scoreText.color = Color.white;
        }
    }

    // Optional: Reset score
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}
