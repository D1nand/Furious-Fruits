using UnityEngine;
using UnityEngine.UI;
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
    public GameObject cloneSpawn;
    public GameObject gameOverCanvas;
    public GameObject livesUI;
    public Sprite starSpriteOutline;

    private bool gameOver = false;
    private bool fruitSpawned = false;
    private bool hasSpawned = false;
    private int spawnedFruitCount = 0; // Counter for spawned fruits

    void Update()
    {
        if (!gameOver && transform.childCount == 0) // if gameObject EnemyManager is empty
        {
            StartCoroutine(ActivateRoundOver()); // calls function
            gameOver = true; // changes boolean to true
        }
        else if (fruit.GetComponent<Fruit>().HasReleased() && !gameOver && !fruitSpawned && spawnPosition != null && spawnedFruitCount < 3)
        { // if fruit is released but the game isn't over and there hasn't been another fruit spawned

            StartCoroutine(SpawnFruitAfterDelay(10f)); // calls function with 10 seconds delay
            fruitSpawned = true; // changes boolean to true
        }
        if(spawnedFruitCount > 2 && !roundOver.activeSelf)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    IEnumerator ActivateRoundOver()
    {
        yield return new WaitForSeconds(2f); // delays for 2 seconds
        if (roundOver != null)
        {
            roundOver.SetActive(true); // activates canvas

            // Get the panel containing the images
            Transform panel = roundOver.transform.Find("Panel");

            if (panel != null)
            {
                // Get the number of children (images) in the panel
                int childCount = panel.childCount;

                if (childCount > 0)
                {

                    if (spawnedFruitCount > 0)
                    {
                        // Access the last image
                        Image lastImage = panel.GetChild(childCount - 1).GetComponent<Image>();

                        // Change the sprite of the last image
                        lastImage.sprite = starSpriteOutline;

                        // If spawnedFruitCount is 2 or more, modify the second-to-last image
                        if (spawnedFruitCount >= 2)
                        {
                            Image secondToLastImage = panel.GetChild(childCount - 2).GetComponent<Image>();
                            secondToLastImage.sprite = starSpriteOutline;
                        }

                    }
                }
                else
                {
                    Debug.LogWarning("No images found in the panel.");
                }
            }
            else
            {
                Debug.LogWarning("Panel not found in the roundOver canvas.");
            }
        }
    }


    IEnumerator SpawnFruitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnFruit(); // calls function

        hasSpawned = true; // changes boolean to true
        fruitSpawned = false; // Reset fruitSpawned to false after spawning a fruit
        cameraFollow.ResetCameraPosition(); // resets camera position to follow clone.
    }


    void SpawnFruit()
    {
        if (fruit != null && hookRigidbody != null && cloneSpawn != null)
        {
            // Reset the position of the existing fruit to the cloneSpawn position
            fruit.position = cloneSpawn.transform.position;

            // Add the spring joint back
            SpringJoint springJoint = fruit.GetComponent<SpringJoint>();
            if (springJoint == null)
            {
                springJoint = fruit.gameObject.AddComponent<SpringJoint>();
            }
            springJoint.connectedBody = hookRigidbody;

            hasSpawned = true; // Set hasSpawned to true after resetting the fruit
            cameraFollow.ResetCameraPosition(); // Reset camera position to follow fruit.

            spawnedFruitCount++; // Increment spawned fruit count

            // Find the Image component in the Canvas GameObject
            Transform lastChild = livesUI.transform.GetChild(livesUI.transform.childCount - 1);

            if (lastChild != null)
            {
                // Find the Image component in the last child GameObject
                Image imageComponent = lastChild.GetComponent<Image>();

                if (imageComponent != null)
                {
                    // to remove the last image
                    Destroy(imageComponent.gameObject);

                }
            }
        }
        else
        {
            Debug.LogError("Fruit, Hook Rigidbody, or Clone Spawn is not assigned!");
        }
    }

    public bool HasSpawned()
    {
        return hasSpawned;
        // returns boolean
    }
}
