using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Collider fruitCollider; // Collider of fruit
    public Collider obstacleCollider; // Collider of obstacle
    public float deadlyHeight = 1f; // Height at which the cube should fall to its death
    private Rigidbody rb; // Rigidbody component of the cube
    private ScoreManager scoreManager; // Reference to the ScoreManager script

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        scoreManager = FindObjectOfType<ScoreManager>(); // Find the ScoreManager in the scene
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == fruitCollider || collision.collider == obstacleCollider)
        { // Check if collided with fruit or obstacle
            Die(); // Call the method to make the cube die
        }
    }

    void Update()
    {
        if (transform.position.y <= deadlyHeight)
        { // Check if cube's Y position is less than or equal to deadlyHeight
            Die(); // Call the method to make the cube die
        }
    }

    void Die()
    {
        scoreManager.AddScore(100); // Add 100 points to the score
        Destroy(gameObject); // Destroy the cube
    }
}
