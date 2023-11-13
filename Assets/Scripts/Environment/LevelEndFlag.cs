using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndFlag : MonoBehaviour
{
    [SerializeField] float colorTime;

    float colorTimer;
    float loadTimer;

    bool load;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        colorTimer = colorTime;
        anim.SetBool("Green", true);

        load = false;
    }

    void Update()
    {
        colorTimer -= Time.deltaTime;

        if (colorTimer < 0)
        {
            if (anim.GetBool("Green"))
            {
                anim.SetBool("Green", false);
                anim.SetBool("Orange", true);
            }
            else if (anim.GetBool("Orange"))
            {
                anim.SetBool("Orange", false);
                anim.SetBool("Blue", true);
            }
            else if (anim.GetBool("Blue"))
            {
                anim.SetBool("Blue", false);
                anim.SetBool("Yellow", true);
            }
            else if (anim.GetBool("Yellow"))
            {
                anim.SetBool("Green", true);
                anim.SetBool("Yellow", false);
            }

            colorTimer = colorTime;
        }

        if (load)
        {
            loadTimer -= Time.deltaTime;

            if (loadTimer < 0)
                LoadNextLevel();
        }
    }

    public void LoadNextLevelAfter(float _afterTime)
    {
        if (!load)
        {
            loadTimer = _afterTime;
            load = true;
        }
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        int firstPlayableScene = 1;

        if (currentScene == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(firstPlayableScene);
        }
        else
            SceneManager.LoadScene(nextScene);
    }
}