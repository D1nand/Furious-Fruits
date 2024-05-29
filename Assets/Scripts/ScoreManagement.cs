using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI TextMeshProUGUI element for displaying score
    private int score = 0; // Current score

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points; // Increase the score by the given points
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    public int CalculateStars()
    {
        int stars = 0;
        int totalScore = GetScore();

        if (totalScore >= 700)
            stars = 3;
        else if (totalScore >= 500)
            stars = 2;
        else if (totalScore >= 100)
            stars = 1;

        return stars;
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString(); // Update the UI text with the current score
    }
}
