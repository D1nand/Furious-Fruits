using UnityEngine;
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

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString(); // Update the UI text with the current score
    }
}
