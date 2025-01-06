using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Footsteps")]
    // public AudioClip[] footstepSounds;
    private AudioSource footstepSource;
    // public List<AudioClip> grassFS;
    // public List<AudioClip> rockFS;
    // public List<AudioClip> sandFS;

    // enum FSMaterial 
    // {
    //     Grass, Rock, Sand, Empty
    // }
    // void Update() 
    // {
    //     PlayFootstepSounds();    
    // }

    // private FSMaterial SurfaceSelect() 
    // {
    //     RaycastHit hit
    // }
    void Start() 
    {
        footstepSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        PlayFootstepSounds();    
    }

    void PlayFootstepSounds()
    {

        if (playerController.isRunning == true) 
        {
            // AudioClip clip = footstepSounds[(int) Random.Range(0, footstepSounds.Length)];
            // footstepSource.clip = clip;
            footstepSource.volume = Random.Range(0.2f,0.5f);
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            footstepSource.enabled = true;
            // footstepSource.Play();
            // StartCoroutine(SoundCooldown());
        }
        else 
        {
            footstepSource.enabled = false;
        }
    }
}
