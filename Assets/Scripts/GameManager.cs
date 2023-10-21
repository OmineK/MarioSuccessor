using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int playerLifeAmount;
    [SerializeField] float roundTime;

    public bool gameOver;

    float roundTimeTimer;
    int score;

    UIManager ui;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        ui = UIManager.instance;

        roundTimeTimer = roundTime;
        ui.UpdateScoreUI(score);
        ui.UpdateLifesLeftUI(playerLifeAmount);
    }

    void Update()
    {
        RoundTimer();
    }

    void RoundTimer()
    {
        if (roundTimeTimer >= 0)
        {
            roundTimeTimer -= Time.deltaTime;
            ui.UpdateTimeLeftUI(roundTimeTimer);
        }
    }

    public void IncreaseSocre(int _increaseAmount)
    {
        score += _increaseAmount;
        ui.UpdateScoreUI(score);
    }

    public void IncreasePlayerLifeAmount()
    {
        if (playerLifeAmount >= 5)
        {
            playerLifeAmount = 5;
            return;
        }

        if (playerLifeAmount < 5)
            playerLifeAmount++;

        ui.UpdateLifesLeftUI(playerLifeAmount);
    }

    public void DecreasePlayerLifeAmount()
    {
        playerLifeAmount--;

        ui.UpdateLifesLeftUI(playerLifeAmount);

        if (playerLifeAmount <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("You lose");
    }
}
