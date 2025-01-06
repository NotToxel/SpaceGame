using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Footsteps")]
    private AudioSource footstepSource;

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
            footstepSource.volume = Random.Range(0.2f,0.5f);
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            footstepSource.enabled = true;
        }
        else 
        {
            footstepSource.enabled = false;
        }
    }
}
