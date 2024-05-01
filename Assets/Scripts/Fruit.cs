using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody rb;
    public float releaseTime = 0.1f;
    public TrailRenderer trailRenderer;

    private bool isPressed = false;
    private SpringJoint springJoint;
    private CameraFollow cameraFollow;
    private bool hasReleased = false;


    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>(); // Assuming the camera is tagged as "MainCamera"
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (isPressed)
        {
            // Convert mouse position to a point in the world
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            

            // Move the ball towards the mouse position
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));
        }
    }
    void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
    }
    void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;

        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);

        hasReleased = true;
        trailRenderer.enabled = true;
        Destroy(springJoint);
    }

    public bool HasReleased()
    {
        return hasReleased;
    }
}
