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
    public int springForce;
    public Rigidbody hook;
    public LineRenderer aimingLine; // Add LineRenderer reference

    private bool isPressed = false;
    private SpringJoint springJoint;

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
        aimingLine.enabled = false; // Disable aiming line at start
    }

    void Update()
    {
        if (isPressed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));
            UpdateAimingLine(mousePosition); // Update aiming line
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
            aimingLine.enabled = true; // Enable aiming line when mouse is pressed
        }
    }

    void OnMouseUp()
    {
        if (!hasReleased)
        {
            isPressed = false;
            rb.isKinematic = false;
            StartCoroutine(Release());
            aimingLine.enabled = false; // Disable aiming line when mouse is released
        }
    }

    void UpdateAimingLine(Vector3 targetPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, targetPosition - spawnPoint.position, out hit))
        {
            if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Floor"))
            {
                aimingLine.SetPosition(0, spawnPoint.position);
                aimingLine.SetPosition(1, hit.point);
            }
            else if (hit.collider.CompareTag("Hook"))
            {
                // Do nothing or handle the hook collision as desired
            }
        }
        else
        {
            aimingLine.SetPosition(0, spawnPoint.position);
            aimingLine.SetPosition(1, targetPosition);
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
}
