using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("--------- Audio Clip ---------")]
    public AudioClip background;
    public AudioClip gameOver;
    public AudioClip itemCollected;
    public AudioClip jump;
    public AudioClip failedFromPlatform;
    public AudioClip win;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
