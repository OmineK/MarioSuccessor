using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGameButton()
    {
        int firstPlayableLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(firstPlayableLevel);
    }

    public void ExitGameButtonUI()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
