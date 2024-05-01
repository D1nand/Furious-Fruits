using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Collider fruitCollider; // asks for the collider of fruit
    public Collider obstacleCollider; // asks for the collider of obstacle
    public float deadlyHeight = 1f;
    private Rigidbody rb; // Rigidbody component of the cube

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == fruitCollider || collision.collider == obstacleCollider)
        { // checks if they collide with Enemy
            Destroy(gameObject); // removes enemy from the game / dies
        }
    }

    void Update()
    {
        if (transform.position.y <= deadlyHeight)
        { // Check if cube's Y position is less than or equal to deadlyHeight
            FallToDeath(); // Call the method to make the cube fall to its death
        }
    }

        void FallToDeath()
        {
            rb.useGravity = true; // Enable gravity to make the cube fall
                                  // You might want to add additional effects or animations here
            Destroy(gameObject); // Destroy the cube after a delay
        }
}
