using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestScore;

    void Start()
    {
        Time.timeScale = 1;

        bestScore.text = "Best game score: " + PlayerPrefs.GetInt("bestScore");
    }

    public void StartGameButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        int firstPlayableLevel = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("roundStartingScore", 0);
        PlayerPrefs.SetInt("roundStartingLifeAmount", 3);
        PlayerPrefs.SetInt("playerStage", 1);

        SceneManager.LoadScene(firstPlayableLevel);
    }

    public void ExitGameButtonUI()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
