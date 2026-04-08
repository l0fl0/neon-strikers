using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadResults()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Results");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}