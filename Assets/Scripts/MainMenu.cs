using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // when clicked on "Play" it loads level01
    }
    public void Quit()
    {
        Application.Quit(); // only works when not locally tested, but it quits the game
    }
}
