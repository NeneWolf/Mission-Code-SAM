using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMainMenu : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Credits()
    {
        SceneManager.LoadScene(3);
    }
}
