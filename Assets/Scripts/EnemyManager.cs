using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject roundOver;
    private bool gameOver = false;

    void Update()
    {
        if (!gameOver && transform.childCount == 0)
        {
            StartCoroutine(ActivateRoundOver());
            gameOver = true;
        }
    }

    IEnumerator ActivateRoundOver()
    {
        yield return new WaitForSeconds(2f);
        if (roundOver != null)
        {
            roundOver.SetActive(true);
        }
    }
}
