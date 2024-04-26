using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform fruit;
    public Vector3 offset;
    public Camera cinematicCamera;
    public Camera mainCamera;
    public EnemyManager enemyManager;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // sets the original position to the position i placed him in the start
    }

    void Update()
    {
        if (enemyManager != null && !enemyManager.HasSpawned()) // Check if enemyManager exists and has not spawned
        {
            if (fruit != null && fruit.GetComponent<Fruit>() != null)
            { // checks if fruit exists
                if (fruit.GetComponent<Fruit>().HasReleased())
                { // checks if the boolean is true
                    mainCamera.gameObject.SetActive(true);
                    cinematicCamera.gameObject.SetActive(false);

                    transform.position = fruit.position + offset;
                }
            }
        }
        else
        {
            ResetCameraPosition(); // calls function
        }
    }


    public void ResetCameraPosition()
    {
        transform.position = originalPosition; // sets camera function to the original
    }
}
