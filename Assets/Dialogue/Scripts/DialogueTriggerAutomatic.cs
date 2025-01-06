// Inspired by Brackeys on YouTube
// https://www.youtube.com/watch?v=_nRzoTzeyxU&t=186s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerAutomatic : MonoBehaviour
{
    public Dialogue dialogue;
    
    void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
