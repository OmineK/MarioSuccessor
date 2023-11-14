using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float roundTime;

    [Header("Current player stage info")]
    public bool playerStage1;
    public bool playerStage2;
    public bool playerStage3;

    [NonSerialized] public bool gameOver;
    [NonSerialized] public float roundTimeTimer;
    [NonSerialized] public int scoreOnRoundStart;
    [NonSerialized] public int score;
    [NonSerialized] public int playerLifeAmount;

    public bool stopRoundTimer;

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
        stopRoundTimer = false;

        score = PlayerPrefs.GetInt("roundStartingScore");
        playerLifeAmount = PlayerPrefs.GetInt("roundStartingLifeAmount");

        scoreOnRoundStart = score;

        ui.UpdateScoreUI(score);
        ui.UpdateLifesLeftUI(playerLifeAmount);
    }

    void Update()
    {
        RoundTimer();
    }

    void RoundTimer()
    {
        if (roundTimeTimer >= 0 && !gameOver && !stopRoundTimer)
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
            IncreaseSocre(1000);
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

    public void UpdatePlayerCurrentStage(bool _stage1, bool _stage2, bool _stage3)
    {
        playerStage1 = _stage1;
        playerStage2 = _stage2;
        playerStage3 = _stage3;

        if (ui != null)
            ui.UpdateCurrentPlayerStageUI(_stage1, _stage2, _stage3);
        else
            StartCoroutine(nameof(UpdateCurrentPlayerStageUIWithDelay));
    }

    IEnumerator UpdateCurrentPlayerStageUIWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if (playerStage1)
            ui.UpdateCurrentPlayerStageUI(true, false, false);
        else if (playerStage2)
            ui.UpdateCurrentPlayerStageUI(false, true, false);
        else if (playerStage3)
            ui.UpdateCurrentPlayerStageUI(false, false, true);
    }

    public void GameOver()
    {
        gameOver = true;

        if (score > PlayerPrefs.GetInt("bestScore"))
            PlayerPrefs.SetInt("bestScore", score);

        UIManager.instance.GameOverPanelActiveUI(true);

        Invoke(nameof(StopTimeAfterGameOver), 2);
    }

    void StopTimeAfterGameOver() => Time.timeScale = 0;
}
