using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Collider fruitCollider; // asks for the collider of fruit
    public Collider obstacleCollider; // asks for the collider of obstacle

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == fruitCollider || collision.collider == obstacleCollider)
        { // checks if they collide with Enemy
            Destroy(gameObject); // removes enemy from the game / dies
        }
    }
}
