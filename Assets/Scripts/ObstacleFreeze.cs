using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFreeze : MonoBehaviour
{
    public Transform fruit;
    public Rigidbody rb;
    void Start()
    {
    }

    
    void Update()
    {
        if (fruit != null && fruit.GetComponent<Fruit>() != null && fruit.GetComponent<Fruit>().HasReleased())
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
