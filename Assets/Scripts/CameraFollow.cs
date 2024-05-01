using UnityEngine;
using TMPro;

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
        Fruit fruitScript = fruit.GetComponent<Fruit>();
        if (enemyManager != null && !enemyManager.HasSpawned())
        {
            if (fruit != null && fruit.GetComponent<Fruit>() != null)
            {
                if (fruitScript.HasReleased())
                {
                    if (!fruitScript.Reset()) {
                        mainCamera.gameObject.SetActive(true);
                        cinematicCamera.gameObject.SetActive(false);

                        transform.position = fruit.position + offset;
                    }
                }
                else
                {
                    // Reset the camera position when the fruit is reset
                    ResetCameraPosition();
                }
            }
        }
        else
        {
            ResetCameraPosition();
        }
        if (!fruitScript.Reset() && fruitScript.HasReleased())
        {
            transform.position = fruit.position + offset;
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
