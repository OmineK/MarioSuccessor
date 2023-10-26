using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Image[] lifesLeftUI;
    [SerializeField] TextMeshProUGUI timeLeftUI;
    [SerializeField] TextMeshProUGUI scoreUI;

    [Header("Player current stage info")]
    [SerializeField] Image stage1;
    [SerializeField] Image stage2;
    [SerializeField] Image stage3;
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void UpdateLifesLeftUI(int _currentLifes)
    {
        foreach (Image life in lifesLeftUI)
        {
            life.enabled = false;
        }

        if (_currentLifes > 0)
        {
            for (int i = 0; i < _currentLifes; i++)
            {
                lifesLeftUI[i].enabled = true;
            }
        }
    }

    public void UpdateTimeLeftUI(float _timeLeft) => timeLeftUI.text = "Time\n" + _timeLeft.ToString(format: "0");

    public void UpdateScoreUI(int _score) => scoreUI.text = "Score\n" + _score.ToString();

    public void UpdateCurrentPlayerStageUI(bool _stage1, bool _stage2, bool _stage3)
    {
        if (_stage1)
        {
            stage1.color = activeColor;
            stage2.color = inactiveColor;
            stage3.color = inactiveColor;
        }

        if (_stage2)
        {
            stage1.color = inactiveColor;
            stage2.color = activeColor;
            stage3.color = inactiveColor;
        }

        if (_stage3)
        {
            stage1.color = inactiveColor;
            stage2.color = inactiveColor;
            stage3.color = activeColor;
        }
    }
}
