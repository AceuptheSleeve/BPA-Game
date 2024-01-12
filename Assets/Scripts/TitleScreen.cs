using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public string gameScene;

    public void StartGame() // Starts the game
    {
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame() // Quits the game
    {
        Application.Quit();
    }
}
