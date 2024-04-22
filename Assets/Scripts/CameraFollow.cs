using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform fruit;
    public Vector3 offset;

    public void Update()
    {
        if (fruit != null && fruit.GetComponent<Fruit>().HasReleased())
        {
            FollowAfterRelease();

        }
    }
    public void FollowAfterRelease()
    {
        transform.position = fruit.position + offset;
    }
}

