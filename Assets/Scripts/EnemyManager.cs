using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject roundOver; // canvas
    public GameObject fruitPrefab; // fruit prefab
    public Transform fruit; // fruit position
    public Transform spawnPosition; // spawn position for clone
    public Camera mainCamera; // main camera
    public CameraFollow cameraFollow; // CameraFollow script
    public Rigidbody hookRigidbody; // rigidbody of the hook used for clone

    private bool gameOver = false;
    private bool fruitSpawned = false;
    private bool hasSpawned = false;

    void Update()
    {
        if (!gameOver && transform.childCount == 0) // if gameObject EnemyManager is empty
        {
            StartCoroutine(ActivateRoundOver()); // calls function
            gameOver = true; // changes boolean to true
        }
        else if (fruit.GetComponent<Fruit>().HasReleased() && !gameOver && !fruitSpawned && spawnPosition != null)
        { // if fruit is released but the game isn't over and there hasn't been another fruit spawned
            
            StartCoroutine(SpawnFruitAfterDelay(10f)); // calls function with 10 seconds delay
            fruitSpawned = true; // changes boolean to true
        }
    }

    IEnumerator ActivateRoundOver()
    {
        yield return new WaitForSeconds(2f); // delays for 2 seconds
        if (roundOver != null)
        {
            roundOver.SetActive(true); // activates canvas
        }
    }

    // kdfjks
    IEnumerator SpawnFruitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnFruit(); // calls function

        hasSpawned = true; // changes boolean to true
        cameraFollow.ResetCameraPosition(); // resets camera position to follow clone.
    }

    void SpawnFruit()
    {
        if (fruitPrefab != null && fruit != null && hookRigidbody != null)
        { // checks if fruitprefab fruit and hookRigidbody exist
            GameObject newFruit = Instantiate(fruitPrefab, spawnPosition.position, Quaternion.identity);
            SpringJoint springJoint = newFruit.AddComponent<SpringJoint>();
            springJoint.connectedBody = hookRigidbody;
            // creates new fruit and connects the springJoint to the hook
        }
        else
        {
            Debug.LogError("FruitPrefab, Fruit, or Hook Rigidbody is not assigned!");
        }
    }
    public bool HasSpawned()
    {
        return hasSpawned;

        // returns boolean
    }
}
