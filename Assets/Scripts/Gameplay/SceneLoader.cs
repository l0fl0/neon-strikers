using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    public void LoadGame()
    {
        MatchData.ResetMatch();
        SceneManager.LoadScene("Arena_01");
    }

    public void LoadArena02()
    {
        SceneManager.LoadScene("Arena_02");
    }

    public void LoadArena03()
    {
        SceneManager.LoadScene("Arena_03");
    }

    public void LoadResults()
    {
        SceneManager.LoadScene("Results");
    }

    public void LoadMainMenu()
    {
        MatchData.ResetMatch();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}