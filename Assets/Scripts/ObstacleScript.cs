using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float heightChangeThreshold = -1.2f; // Threshold for height change
    public int points = 100; // Points to be awarded when the obstacle is destroyed
    private ScoreManager scoreManager; // Reference to the ScoreManager script
    private float initialHeight; // Initial height of the obstacle

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>(); // Find the ScoreManager in the scene
        initialHeight = transform.position.y; // Record the initial height
    }

    void Update()
    {
        // Check if the obstacle's height has changed by the threshold amount
        if (transform.position.y <= initialHeight + heightChangeThreshold)
        {
            HandleDestruction();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the obstacle was hit by an object tagged as "Fruit"
        if (collision.collider.CompareTag("Fruit"))
        {
            HandleDestruction();
        }
    }

    void HandleDestruction()
    {
        // Add points to the score
        if (scoreManager != null)
        {
            scoreManager.AddScore(points);
        }

        // Destroy the obstacle
        Destroy(gameObject);
    }
}
