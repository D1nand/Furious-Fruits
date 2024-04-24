using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Collider fruitCollider;
    public Collider obstacleCollider;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == fruitCollider || collision.collider == obstacleCollider)
        {
            Destroy(gameObject);
        }
    }
}
