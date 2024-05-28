using System.Collections;
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
    public LineRenderer lineRenderer;
    public int trajectoryResolution = 30;
    public float springForce = 50f;
    public LayerMask collisionMask;

    private bool isPressed = false;
    private SpringJoint springJoint;

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
        lineRenderer.positionCount = trajectoryResolution;
    }

    void Update()
    {
        if (isPressed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));
            DrawTrajectory(mousePosition);
        }
        else
        {
            lineRenderer.positionCount = 0;
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
            springJoint.spring = springForce;
            springJoint.autoConfigureConnectedAnchor = false;
        }
    }

    void DrawTrajectory(Vector3 aimPosition)
    {
        Vector3[] points = new Vector3[trajectoryResolution];
        Vector3 startingPosition = transform.position;
        Vector3 startingVelocity = CalculateVelocity(startingPosition, aimPosition);
        points[0] = startingPosition;

        for (int i = 1; i < trajectoryResolution; i++)
        {
            float time = i * (releaseTime / trajectoryResolution);
            Vector3 point = CalculatePositionAtTime(startingPosition, startingVelocity, time);
            points[i] = point;

            if (Physics.Linecast(points[i - 1], points[i], out RaycastHit hit, collisionMask))
            {
                points[i] = hit.point;
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPositions(points);
                return;
            }
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    Vector3 CalculateVelocity(Vector3 startPosition, Vector3 aimPosition)
    {
        Vector3 direction = aimPosition - startPosition;
        float distance = direction.magnitude;
        float mass = rb.mass;
        float velocityMagnitude = Mathf.Sqrt(springForce * distance * 2 / mass);
        return direction.normalized * velocityMagnitude;
    }

    Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 startVelocity, float time)
    {
        Vector3 gravity = Physics.gravity;
        return startPosition + startVelocity * time + 0.5f * gravity * time * time;
    }

    public bool HasReleased()
    {
        return hasReleased;
    }

    public bool Reset()
    {
        return reset;
    }
}
