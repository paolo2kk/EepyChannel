using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    public AudioSource SFXSource;
    public AudioSource MusicSource;

    public AudioClip music;
    public AudioClip reset;
    public AudioClip jumpsound;

    private void Awake()
    {
       
       MusicSource.clip = music;
       MusicSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    
}
