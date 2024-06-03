using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody rb;
    public float releaseTime = 0.2f;
    public TrailRenderer trailRenderer;
    public Transform spawnPoint;
    public bool reset = false;
    public bool hasReleased = false;
    public Rigidbody hook;
    public float springForce = 50f;
    public LineRenderer lineRenderer;
    public int lineSegmentCount = 20;
    public float trajectoryDuration = 2f;

    private bool isPressed = false;
    private SpringJoint springJoint;
    private Camera mainCamera;
    private float mass = 0.3f;
    private float drag = 0.3f;
    private float angularDrag = 1f;

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
        mainCamera = Camera.main;
        lineRenderer.positionCount = lineSegmentCount;

        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
    }

    void Update()
    {
        if (isPressed)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));

            UpdateTrajectory();
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (reset)
        {
            StartCoroutine(Wait());
            reset = false;
            hasReleased = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }

    void OnMouseDown()
    {
        if (!hasReleased)
        {
            isPressed = true;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void OnMouseUp()
    {
        if (!hasReleased)
        {
            isPressed = false;
            rb.isKinematic = false;

            StartCoroutine(Release());
        }
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);

        hasReleased = true;
        trailRenderer.enabled = true;
        Destroy(springJoint);

        yield return new WaitForSeconds(10f);

        if (!reset)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = spawnPoint.position;
            hasReleased = false;
            trailRenderer.enabled = false;
            reset = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = hook;
            springJoint.spring = 200f;
            springJoint.autoConfigureConnectedAnchor = false;
        }
    }

    public bool HasReleased()
    {
        return hasReleased;
    }

    public bool Reset()
    {
        return reset;
    }

    void UpdateTrajectory()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = (hook.position - transform.position) * springForce / mass;

        Vector3[] points = new Vector3[lineSegmentCount];
        points[0] = currentPosition;

        float timeStep = trajectoryDuration / (float)lineSegmentCount;
        for (int i = 1; i < lineSegmentCount; i++)
        {
            float currentTime = i * timeStep;
            Vector3 displacement = currentVelocity * currentTime + 0.5f * Physics.gravity * currentTime * currentTime;
            points[i] = currentPosition + displacement;
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }



    Vector3 CalculateDisplacement(Vector3 initialVelocity, float time)
    {
        Vector3 gravity = Physics.gravity;
        Vector3 dragForce = -rb.velocity.normalized * drag * rb.velocity.magnitude;
        Vector3 acceleration = gravity + (dragForce / mass);
        Vector3 displacement = initialVelocity * time + 0.5f * acceleration * time * time;
        return displacement;
    }


}
