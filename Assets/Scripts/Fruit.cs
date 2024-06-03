using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody rb;
    public float releaseTime = 0.2f;
    public TrailRenderer trailRenderer;
    public Transform spawnPoint; // New field to hold the spawn point for the fruit
    public bool reset = false;
    public bool hasReleased = false;
    public Rigidbody hook;
    public float springForce = 50f; // Spring force of the SpringJoint
    public LineRenderer lineRenderer; // LineRenderer for the aiming system
    public int lineSegmentCount = 20; // Number of segments in the line renderer
    public float trajectoryDuration = 2f; // Duration for which to simulate the trajectory

    private bool isPressed = false;
    private SpringJoint springJoint;
    private Camera mainCamera;
    private float mass = 0.3f; // Mass of the fruit
    private float drag = 0.3f; // Drag of the fruit
    private float angularDrag = 1f; // Angular drag of the fruit

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
        mainCamera = Camera.main;
        lineRenderer.positionCount = lineSegmentCount; // Set the number of positions for the LineRenderer

        // Set the Rigidbody properties
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

            // Update LineRenderer positions for the trajectory
            UpdateTrajectory();
            lineRenderer.enabled = true; // Enable the LineRenderer when aiming
        }
        else
        {
            lineRenderer.enabled = false; // Disable the LineRenderer when not aiming
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

        // Respawn the fruit after a delay
        yield return new WaitForSeconds(10f); // Adjust the delay as needed

        if (!reset)
        {
            // Reset the position of the fruit to the spawn point
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = spawnPoint.position;
            hasReleased = false; // Reset hasReleased flag
            trailRenderer.enabled = false; // Disable the trail renderer
            reset = true;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = hook;
            springJoint.spring = springForce; // Set the spring strength
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
        Vector3 velocity = (hook.position - transform.position) * springForce / mass;
        Vector3 currentPosition = transform.position;

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float simulationTime = i / (float)lineSegmentCount * trajectoryDuration;
            Vector3 displacement = CalculateDisplacement(velocity, simulationTime);
            Vector3 drawPoint = currentPosition + displacement;
            lineRenderer.SetPosition(i, drawPoint);
        }
    }

    Vector3 CalculateDisplacement(Vector3 initialVelocity, float time)
    {
        // Calculate the drag force
        float dragFactor = Mathf.Exp(-drag * time / mass);
        // Calculate the velocity considering drag
        Vector3 velocityWithDrag = initialVelocity * dragFactor;
        // Calculate the displacement considering gravity and drag
        Vector3 displacement = velocityWithDrag * time + 0.5f * Physics.gravity * time * time;
        return displacement;
    }
}
