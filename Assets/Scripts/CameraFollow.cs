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
        originalPosition = transform.position;
    }

    void Update()
    {
        if (enemyManager != null && !enemyManager.HasSpawned()) // Check if enemyManager exists and has not spawned
        {
            if (fruit != null && fruit.GetComponent<Fruit>() != null)
            {
                if (fruit.GetComponent<Fruit>().HasReleased())
                {
                    mainCamera.gameObject.SetActive(true);
                    cinematicCamera.gameObject.SetActive(false);

                    transform.position = fruit.position + offset;
                }
            }
        }
        else
        {
            ResetCameraPosition();
        }
    }


    public void ResetCameraPosition()
    {
        transform.position = originalPosition;
    }
}
