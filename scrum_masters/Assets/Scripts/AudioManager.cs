using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource SFXSource;

    public AudioClip reset;
    public AudioClip jumpsound;
    public AudioClip background;

    private void Awake()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
}
