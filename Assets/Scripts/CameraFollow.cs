using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform fruit;
    public Vector3 offset;
    public Camera cinematicCamera;
    public Camera mainCamera;


    void Update()
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
}
