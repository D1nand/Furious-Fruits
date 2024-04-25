using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject roundOver;
    public GameObject fruitPrefab;
    public Transform fruit;
    public Transform spawnPosition;
    public Camera mainCamera; // Reference to the main camera
    public CameraFollow cameraFollow;
    public Rigidbody hookRigidbody; // Public reference to the Rigidbody of the hook

    private bool gameOver = false;
    private bool fruitSpawned = false;
    private bool hasSpawned = false;

    void Update()
    {
        if (!gameOver && transform.childCount == 0)
        {
            StartCoroutine(ActivateRoundOver());
            gameOver = true;
        }
        else if (fruit.GetComponent<Fruit>().HasReleased() && !gameOver && !fruitSpawned && spawnPosition != null)
        {
            // Spawn fruit only if there are enemies and a fruit has not already been spawned
            StartCoroutine(SpawnFruitAfterDelay(10f));
            fruitSpawned = true; // Set flag to indicate fruit has been spawned
        }
    }

    IEnumerator ActivateRoundOver()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        if (roundOver != null)
        {
            roundOver.SetActive(true);
        }
    }

    IEnumerator SpawnFruitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnFruit();
        // Reset the main camera position

        hasSpawned = true;
        cameraFollow.ResetCameraPosition();
    }

    void SpawnFruit()
    {
        if (fruitPrefab != null && fruit != null && hookRigidbody != null)
        {
            GameObject newFruit = Instantiate(fruitPrefab, spawnPosition.position, Quaternion.identity);
            SpringJoint springJoint = newFruit.AddComponent<SpringJoint>();
            springJoint.connectedBody = hookRigidbody;
        }
        else
        {
            Debug.LogError("FruitPrefab, Fruit, or Hook Rigidbody is not assigned!");
        }
    }
    public bool HasSpawned()
    {
        return hasSpawned;
    }
}
