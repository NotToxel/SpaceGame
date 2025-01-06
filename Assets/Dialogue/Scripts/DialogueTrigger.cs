// Inspired by Brackeys on YouTube
// https://www.youtube.com/watch?v=_nRzoTzeyxU&t=186s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
