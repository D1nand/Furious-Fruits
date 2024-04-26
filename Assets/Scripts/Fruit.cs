using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Rigidbody rb; //  rigidbody of the fruit
    public float releaseTime = 0.2f; // releaseTime set to 0.2f
    public TrailRenderer trailRenderer; // TrailRenderer of the fruit

    private bool isPressed = false;
    private SpringJoint springJoint; // SpringJoint connected to the fruit
    private CameraFollow cameraFollow; //  CameraFollow script
    private bool hasReleased = false;


    void Start()
    {
        springJoint = GetComponent<SpringJoint>(); //  calls for the springjoint component connected to the fruit
        cameraFollow = Camera.main.GetComponent<CameraFollow>(); // defines camerafollow as the script from main camera
        rb.constraints = RigidbodyConstraints.FreezePosition; // freezes the "fruit" because otherwise it would look weird
        trailRenderer.enabled = false; // trailrenderer is not enabled. because when we pull the ball back we dont want to see a trail
    }

    void Update()
    {
        if (isPressed)
        { // checks if the boolean is true
            // Convert mouse position to a point in the world
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            

            // Move the ball towards the mouse position
            rb.MovePosition(Vector3.Lerp(rb.position, mousePosition, Time.deltaTime * 10f));
        }
    }
    void OnMouseDown()
    { // if mousebutton is holded down
        isPressed = true; // sets boolean to true
        rb.isKinematic = true; // sets kinematic on the fruits rigidbody to true
        rb.constraints = RigidbodyConstraints.None; // removes the freeze from the ball so it can move
    }
    void OnMouseUp()
    { // if mousebutton is released
        isPressed = false; // changes boolean back to false
        rb.isKinematic = false; // changes kinematic on the fruits rigidbody back to false

        StartCoroutine(Release()); // calls function
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime); // waits to do all the other code underneath

        hasReleased = true; // changes hasReleased boolean to true
        trailRenderer.enabled = true; //  enables the trailRenderer
        Destroy(springJoint); // destroys springjoint so the ball actually flies away and doesn't come back
    }

    public bool HasReleased()
    {
        return hasReleased; // defines boolean hasReleased
    }
}
