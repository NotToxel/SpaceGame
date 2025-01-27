// Inspired by Brackeys on YouTube
// https://www.youtube.com/watch?v=_nRzoTzeyxU&t=186s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences;
    public GameObject camera;
    public FirstPersonCamera cameraScript;
    public GameObject inputManager;
    public PlayerController playerController;
    public AudioSource nextSentenceSound;
    public AudioSource startSentenceSound;

    public Animator animator;
    public bool inDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue) 
    {
        startSentenceSound.Play();
        inDialogue = true;
        // Disable camera controls
        if (camera != null) {
            FirstPersonCamera cameraScript = FindObjectOfType<FirstPersonCamera>();
            if (cameraScript != null) { cameraScript.DisableCam(); }
        }
        else { Debug.Log("camera is null"); }



        // Enable cursor controls
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        animator.SetBool("isOpen", true);
        //Debug.Log("Should be showing");

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        nextSentenceSound.Play();
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue() 
    {
        // Enable camera controls
        if (camera != null) {
            FirstPersonCamera cameraScript = FindObjectOfType<FirstPersonCamera>();
            if (cameraScript != null) { cameraScript.EnableCam(); }
        }
        else { Debug.Log("camera is null"); }

        // // Disable cursor controls
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inDialogue = false;
        animator.SetBool("isOpen", false);
        //firstPersonCamera.currentMouseSensitivity = firstPersonCamera.mouseSensitivity;
    }
}
