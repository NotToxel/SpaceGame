using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip openTablet;
    public AudioClip closeTablet;

    // Start is called before the first frame update
    void Start()
    {
        // musicSource.clip = background;
        // musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) 
    {
        SFXSource.PlayOneShot(clip);
    }
}
