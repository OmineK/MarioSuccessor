using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int score;
    [SerializeField] int playerLifeAmount;

    public bool gameOver;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void IncreaseSocre(int _increaseAmount) => score += _increaseAmount;

    public void IncreasePlayerLifeAmount()
    {
        if (playerLifeAmount >= 5)
        {
            playerLifeAmount = 5;
            return;
        }

        if (playerLifeAmount < 5)
            playerLifeAmount++;
    }

    public void DecreasePlayerLifeAmount()
    {
        playerLifeAmount--;

        if (playerLifeAmount <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("You lose");
    }
}
