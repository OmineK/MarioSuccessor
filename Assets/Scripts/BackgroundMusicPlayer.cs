using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    void Awake()
    {
        int numCurrentMusicPlays = FindObjectsOfType<BackgroundMusicPlayer>().Length;

        if (numCurrentMusicPlays > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
}
