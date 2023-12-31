using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioClip[] sfx;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void PlaySFX(int _sfxIndex) => audioSource.PlayOneShot(sfx[_sfxIndex]);
}
