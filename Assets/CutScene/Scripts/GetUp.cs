using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetUp : MonoBehaviour
{
    public Animator animator;
    public GameObject dialogue;
    public float duration1;
    public float duration2;
    public float duration3;
    public GameObject player;
    public GameObject cam;
    public QuestCatalyst quest;
    
    void Start()
    {
        StartCoroutine(NextAnimation(duration1));
    }

    IEnumerator NextAnimation(float duration1)
    {
        yield return new WaitForSeconds(duration1);
        animator.SetBool("sitOnBed", true);
        yield return new WaitForSeconds(duration2);
        animator.SetBool("standUp", true);
        yield return new WaitForSeconds(duration3);
        player.SetActive(true);
        dialogue.SetActive(true);
        quest.CreateQuest();
        cam.SetActive(false);
    }

}
