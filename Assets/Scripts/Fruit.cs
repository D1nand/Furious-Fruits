using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody rb;
    public float releaseTime = 0.1f;
    public TrailRenderer trailRenderer;
    public Transform spawnPoint; // New field to hold the spawn point for the fruit
    public bool reset = false;
    public bool hasReleased = false;
    public Rigidbody hook;

    private bool isPressed = false;
    private SpringJoint springJoint;
    

    void Start()
    {
        springJoint = GetComponent<SpringJoint>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        trailRenderer.enabled = false;
    }

    void Update()
    {
        if (isPressed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));
        }
        if (reset)
        {
            Wait();
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
            springJoint.spring = 30f; // Set the spring strength
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
