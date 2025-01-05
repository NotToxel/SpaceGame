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
    public FirstPersonCamera firstPersonCamera;

    public GameObject inputManager;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        // firstPersonCamera.currentMouseSensitivity = 0f;
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        animator.SetBool("isOpen", true);
        Debug.Log("Should be showing");

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
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
        animator.SetBool("isOpen", false);
        firstPersonCamera.currentMouseSensitivity = firstPersonCamera.mouseSensitivity;
        inputManager.SetActive(true);
    }
}
